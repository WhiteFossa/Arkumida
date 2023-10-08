using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using furtails_importer.Dbos;
using furtails_importer.Helpers;
using furtails_importer.Models;
using furtails_importer.WebClientStuff.Dtos;
using furtails_importer.WebClientStuff.Enums;
using furtails_importer.WebClientStuff.Requests;
using furtails_importer.WebClientStuff.Responses;
using MySqlConnector;

namespace furtails_importer.Importers;

public class TextsImporter
{
    private readonly MySqlConnection _connection;
    private readonly HttpClient _httpClient;
    private readonly UsersImporter _usersImporter;

    public TextsImporter(MySqlConnection connection, HttpClient httpClient, UsersImporter usersImporter)
    {
        _connection = connection;
        _httpClient = httpClient;
        _usersImporter = usersImporter;
    }

    public async Task Import()
    {
        var categories = LoadCategories(_connection);
        
        var texts = _connection.Query<FtText>
            (
                @"select
                    id as Id,
                    categoryId as CategoryId,
                    uid as UploaderUserId,
                    username as UploaderUserName,
                    file_create_date as CreateTime,
                    file_change_date as UpdateTime,
                    file_title as Title,
                    file_info as Description,
                    file_author as Author,
                    file_transletor as Translator,
                    isHidden as IsHidden,
                    file_size as Size,
                    file_section as FileSection,
                    tid as Tid,
                    seriesId as SeriesId,
                    count_vote_plus as VotesPlus,
                    count_vote_minus as VotesMinus,
                    count_reads as ReadsCount,
                    count_comments as CommentsCount,
                    count_vote as VotesCount,
                    baseType as BaseType,
                    `type` as Type,
                    isDeleted as IsDeleted,
                    isPublished as PublishStatus,
                    contestId as ContestId,
                    isAnonim as IsAnonymous,
                    isNotFinished as IsNotFinished
                from ft_objects"
            )
            .ToList();
        
        // Importing creatures, who are authors, editors and translators
        var creaturesFromTexts = new List<string>();

        foreach (var text in texts)
        {
            if (text.IsDeleted == 0 && text.PublishStatus == 2)
            {
                creaturesFromTexts.AddRange(await GetCreaturesFromTextAsync(text));   
            }
        }

        creaturesFromTexts = creaturesFromTexts
            .Distinct(StringComparer.InvariantCultureIgnoreCase)
            .ToList();
        
        var parallelismDegree = new ParallelOptions()
        {
            MaxDegreeOfParallelism = MainImporter.ParallelismDegree
        };
        
        await Parallel.ForEachAsync(creaturesFromTexts, parallelismDegree, async (creature, token) =>
        {
            await RegisterUserIfNotExistAsync(creature);
        });
        
        var textsImportParallelismDegree = new ParallelOptions()
        {
            MaxDegreeOfParallelism = MainImporter.TextsImportParallelismDegree
        };
        
        await Parallel.ForEachAsync(texts, textsImportParallelismDegree, async (text, token) =>
        {
            await AddTextToArkumidaAsync(categories, text);
        });
    }

    private async Task<IReadOnlyCollection<string>> GetCreaturesFromTextAsync(FtText text)
    {
        var result = new List<string>();
        
        // Publisher - always exist
        result.Add(text.UploaderUserName.Trim());
            
        // Authors
        if (!string.IsNullOrWhiteSpace(text.Author))
        {
            var authorsNames = text.Author.Split(',').Select(an => an.Trim()); // Trim helps to remove spaces, which can be after comma
            result.AddRange(authorsNames);
        }
            
        // Translators
        if (!string.IsNullOrWhiteSpace(text.Translator))
        {
            var translatorsNames = text.Translator.Split(',').Select(tn => tn.Trim());
            result.AddRange(translatorsNames);
        }

        return result;
    }

    private async Task AddTextToArkumidaAsync(IReadOnlyCollection<FtCategory> categories, FtText text)
    {
        if (text.IsDeleted != 0)
        {
            return; // Deleted text
        }

        if (text.PublishStatus != 2)
        {
            return; // Not published yet
        }
        
        Console.WriteLine($"Text ID: { text.Id }, Title: { text.Title }");
        
        await using var connection = new MySqlConnection(MainImporter.ConnectionString);
        
        var textModel = new Text();

        // We need files metadata early to process comics
        var textFilesMetadata = LoadTextFilesByText(connection, text.Id);
        
        if (text.Type == 3) // Editing
        {
            var sections = LoadWIPTextSections(connection, text.Id);
            
            // One page, many sections, each have one variant
            textModel.Pages = new List<TextPage>();

            var page = new TextPage()
            {
                Id = Guid.Empty,
                Number = 1,
                Sections = new List<TextSection>()
            };
            
            textModel.Pages.Add(page);
            
            page.Sections = new List<TextSection>();
            
            foreach (var section in sections)
            {
                var variantModel = new TextSectionVariant()
                {
                    Content = TextsHelper.FixupText(section.SectionRus),
                    CreationTime = section.SaveDate
                };

                var sectionModel = new TextSection()
                {
                    OriginalText = TextsHelper.FixupText(section.SectionEng),
                    Variants = new List<TextSectionVariant>() { variantModel }
                };
                
                page.Sections.Add(sectionModel);
            }
        }
        else if (text.Type == 5) // Translation
        {
            textModel.Pages = new List<TextPage>();

            var page = new TextPage()
            {
                Id = Guid.Empty,
                Number = 1,
                Sections = new List<TextSection>()
            };
            
            textModel.Pages.Add(page);
            
            // One page, many sections, each have many variants
            page.Sections = new List<TextSection>();
            
            var parts = LoadTranslationParts(connection, text.Id);

            foreach (var part in parts)
            {
                var sectionModel = new TextSection()
                {
                    OriginalText = TextsHelper.FixupText(part.OriginalText),
                    Variants = new List<TextSectionVariant>()
                };
                
                // Loading variants for each section
                var variants = LoadTranslationVariants(connection, part.Id);
                foreach (var variant in variants)
                {
                    sectionModel.Variants.Add(new TextSectionVariant()
                    {
                        Content = TextsHelper.FixupText(variant.Text),
                        CreationTime = variant.CreateTime
                    });
                }
                
                page.Sections.Add(sectionModel);
            }
        }
        else if (text.Type == 1) // Just a text
        {
            textModel.Pages = new List<TextPage>();

            var page = new TextPage()
            {
                Id = Guid.Empty,
                Number = 1,
                Sections = new List<TextSection>()
            };
            
            textModel.Pages.Add(page);
            
            // Ordinary text, all-in-one-section-and-variant
            var variantModel = new TextSectionVariant()
            {
                Content = TextsHelper.FixupText(LoadTextById(text.Id)),
                CreationTime = DateTime.UtcNow
            };

            var sectionModel = new TextSection()
            {
                OriginalText = string.Empty, // There is no English original
                Variants = new List<TextSectionVariant>() { variantModel }
            };

            page.Sections = new List<TextSection>() { sectionModel };
        }
        else if (text.Type == 2) // Links set
        {
            textModel.Pages = new List<TextPage>();

            var page = new TextPage()
            {
                Id = Guid.Empty,
                Number = 1,
                Sections = new List<TextSection>()
            };
            
            textModel.Pages.Add(page);
            
            page.Sections = new List<TextSection>();
            
            // One link per section
            var links = LoadLinksByText(connection, text.Id);

            foreach (var link in links)
            {
                var variantModel = new TextSectionVariant()
                {
                    Content = link.Link,
                    CreationTime = DateTime.UtcNow
                };
                
                var sectionModel = new TextSection()
                {
                    OriginalText = string.Empty, // There is no English original
                    Variants = new List<TextSectionVariant>() { variantModel }
                };
                
                page.Sections.Add(sectionModel);
            }
        }
        else if (text.Type == 4) // Comics
        {
            // Comics - multi pages, each page contains one section, each section contains one variant
            textModel.Pages = new List<TextPage>();

            int pageNumber = 1;
            foreach (var textFile in textFilesMetadata)
            {
                // One page per file
                var page = new TextPage()
                {
                    Number = pageNumber,
                    Sections = new List<TextSection>()
                };
                
                textModel.Pages.Add(page);
                
                var variantModel = new TextSectionVariant()
                {
                    Content = TextsHelper.FixupText($"[cim]{ textFile.Name }[/cim]"), // cim = Comics IMage
                    CreationTime = DateTime.UtcNow
                };
                
                var sectionModel = new TextSection()
                {
                    OriginalText = string.Empty, // There is no English original
                    Variants = new List<TextSectionVariant>() { variantModel }
                };
                
                page.Sections.Add(sectionModel);
                
                pageNumber++;
            }
        }
        
        
        var arkumidaTagsIds = new List<Guid>();
        
        // Adding category tags
        var textCategory = categories.Single(c => c.Id == text.CategoryId);

        arkumidaTagsIds.Add((await GetTagFromArkumidaByNameAsync(textCategory.Name)).Id);
        if (textCategory.Name != "Рассказы" && textCategory.Name != "Повести и Романы" && textCategory.Name != "Стихи" && textCategory.Name != "Комиксы")
        {
            // No size category, in this case we need to add size category manually
            arkumidaTagsIds.Add((await GetTagFromArkumidaByNameAsync("Рассказы")).Id); // TODO: In future, we can try to detect correct size category by analyzing text size
        }

        if (text.Type == 3) // Special category for texts in edit
        {
            arkumidaTagsIds.Add((await GetTagFromArkumidaByNameAsync("Мастерская Гайки")).Id);
        }

        if (text.Type == 4) // Special category for comics
        {
            arkumidaTagsIds.Add((await GetTagFromArkumidaByNameAsync("Комиксы")).Id);
        }

        // Loading tags
        var textTagsRelations = LoadTextsToTagsRelations(connection, text.Id);
        foreach (var tagRelation in textTagsRelations)
        {
            var tagName = LoadTagById(connection, tagRelation.TagId).Name;
            
            // If test have "contest" tag (not to be mistook with "contest" category) we will add text to "contest" category
            if (tagName == "конкурс")
            {
                var contestCategory = await GetTagFromArkumidaByNameAsync("Конкурсные произведения");

                if (!arkumidaTagsIds.Contains(contestCategory.Id))
                {
                    arkumidaTagsIds.Add(contestCategory.Id);
                }
            }
            else
            {
                // Ordinary tag, adding it as is
                var tagToAdd = await GetTagFromArkumidaByNameAsync(tagName);
                arkumidaTagsIds.Add(tagToAdd.Id);
            }
        }

        // Checking do we have users and creating them if not
        
        // Publisher - always exist
        var publisherCreature = await _usersImporter.FindCreatureByLogin(text.UploaderUserName.Trim());
        
        // Authors
        var authors = new List<CreatureDto>();
        if (string.IsNullOrWhiteSpace(text.Author))
        {
            // Special case - no information on authors - use publisher instead
            authors.Add(publisherCreature);
        }
        else
        {
            var authorsNames = text.Author.Split(',').Select(an => an.Trim()); // Trim helps to remove spaces, which can be after comma
            foreach (var authorName in authorsNames)
            {
                authors.Add(await _usersImporter.FindCreatureByLogin(authorName));
            }
        }
        
        // Translators
        var translators = new List<CreatureDto>();
        if (!string.IsNullOrWhiteSpace(text.Translator))
        {
            var translatorsNames = text.Translator.Split(',').Select(tn => tn.Trim());
            foreach (var translatorName in translatorsNames)
            {
                translators.Add(await _usersImporter.FindCreatureByLogin(translatorName));
            }
        }

        // Now we have text model ready
        var textToCreate = new TextDto()
        {
            Id = Guid.Empty,
            CreateTime = text.CreateTime.ToUniversalTime(),
            LastUpdateTime = (text.UpdateTime.HasValue ? text.UpdateTime.Value : text.CreateTime).ToUniversalTime(),
            Title = text.Title,
            Description = text.Description,
            Pages = new Collection<TextPageDto>(),
            ReadsCount = text.ReadsCount,
            VotesCount = text.VotesCount,
            VotesPlus = text.VotesPlus,
            VotesMinus = text.VotesMinus,
            
            Tags = arkumidaTagsIds.Select(tid => new TagDto()
            {
                Id = tid, // Only ID important right now, because tag already exist
                FurryReadableId = "Not important",
                Name = "Not important",
                Subtype = TagSubtype.Actions,
                CategoryOrder = 0,
                IsCategory = false,
                CategoryTagType = CategoryTagType.Normal
            }).ToList(),
            
            IsIncomplete = text.IsNotFinished,
            
            Authors = authors,
            Translators = translators,
            Publisher = publisherCreature
        };

        foreach (var page in textModel.Pages)
        {
            var pageToCreate = new TextPageDto()
            {
                Id = Guid.Empty,
                Number = page.Number,
                Sections = new Collection<TextSectionDto>()
            };
            
            var sectionOrder = 0;
            foreach (var section in page.Sections)
            {
                var sectionToCreate = new TextSectionDto()
                {
                    Id = Guid.Empty,
                    OriginalText = section.OriginalText,
                    Order = sectionOrder,
                    Variants = new List<TextSectionVariantDto>()
                };
            
                pageToCreate
                    .Sections
                    .Add(sectionToCreate);
            
                foreach (var variant in section.Variants)
                {
                    var variantToCreate = new TextSectionVariantDto()
                    {
                        Id = Guid.Empty,
                        Content = variant.Content,
                        CreationTime = variant.CreationTime.ToUniversalTime()
                    };
                
                    sectionToCreate.Variants.Add(variantToCreate);
                }

                sectionOrder++;
            }
            
            textToCreate
                .Pages
                .Add(pageToCreate);
        }

        var arkumidaTextId = await AddTextToArkumidaAsync(textToCreate);
        
        // Now getting files for this text
        foreach (var fileMetadata in textFilesMetadata)
        {
            if (string.IsNullOrWhiteSpace(fileMetadata.Name))
            {
                continue; // Buggy file in old FT
            }

            var path = GenerateTextFilePath(text.Id, fileMetadata.Hash, fileMetadata.SubType);
            var mimeType = GetMimeTypeByFileSubtype(fileMetadata.SubType);

            if (!File.Exists(path))
            {
                continue; // Buggy database - metadata exists, but file - no
            }
            
            var content = await File.ReadAllBytesAsync(path);

            var uploadedFile = await FilesHelper.UploadFileToArkumidaAsync(_httpClient, fileMetadata.Name, mimeType, content);
            
            // Now just attach file to text
            await AddFileToArkumidaTextAsync(arkumidaTextId, fileMetadata.Name, uploadedFile.FileInfo.Id);
        }
    }
    
    private string LoadTextById(int id)
    {
        return File.ReadAllText($@"{MainImporter.TextsDbRoot}/{id}/TEXT");
    }

    private List<FtRawTextSection> LoadWIPTextSections(MySqlConnection connection, int textId)
    {
        return connection.Query<FtRawTextSection>
            (
                @"select
                    id as Id,
                    object_id as TextId,
                    index_number as SectionNumber,
                    val_rus as SectionRus,
                    lock_user_id as LockUserId,
                    lock_date as LockDateTime,
                    var_eng as SectionEng,
                    user_id as TranslatorId,
                    save_date as SaveDate
                from ft_objects_raw_text
                where object_id = @textId",
                new { textId = textId }
            )
            .OrderBy(s => s.SectionNumber)
            .ToList();
    }

    private List<FtTranslateTextPart> LoadTranslationParts(MySqlConnection connection, int textId)
    {
        return connection.Query<FtTranslateTextPart>
            (
                @"select
                    id as Id,
                    ttid as TextId,
                    indexNumber as OrderNumber,
                    val as OriginalText,
                    lockUserId as LockUserId,
                    lockDate as LockTime
                from ft_translate_texts_parts
                where ttid = @textId",
                new { textId = textId }
            )
            .OrderBy(s => s.OrderNumber)
            .ToList();
    }

    private List<FtTranslateTextVariant> LoadTranslationVariants(MySqlConnection connection, int partId)
    {
        return connection.Query<FtTranslateTextVariant>
            (
                @"select
                    id as Id,
                    ttpId as PartId,
                    userId as UserId,
                    createDate as CreateTime,
                    val as Text
                from ft_translate_texts_variant
                where ttpid = @ttpId",
                new { ttpId = partId }
            )
            .OrderBy(s => s.CreateTime)
            .ToList();
    }

    private List<FtTextLink> LoadLinksByText(MySqlConnection connection, int textId)
    {
        return connection.Query<FtTextLink>
            (
                @"select
                    id as Id,
                    objectId as TextId,
                    value as Link,
                    isOK as IsOk,
                    status as ErrorsCount,
                    lastCheck as LastCheck
                from ft_objects_links
                where objectId = @textId",
                new { textId = textId }
            )
            .ToList();
    }

    private List<FtTextTag> LoadTextsToTagsRelations(MySqlConnection connection, int textId)
    {
        return connection.Query<FtTextTag>
            (
                @"select
                    id as Id,
                    fileId as TextId,
                    tagId as TagId
                from ft_objects_tags
                where fileId = @textId",
                new { textId = textId }
            )
            .ToList();
    }

    private FtTag LoadTagById(MySqlConnection connection, int tagId)
    {
        return connection.Query<FtTag>
            (
                @"select
                    id as Id,
                    name as Name,
                    isHidden as IsHidden,
                    isWarning as IsWarning,
                    groupId as GroupId,
                    icon as Icon,
                    access_mode as AccessMode,
                    class as Class
                from ft_tags
                where id = @tagId",
                new { tagId = tagId }
            )
            .Single();
    }
    
    private List<FtCategory> LoadCategories(MySqlConnection connection)
    {
        var result = connection.Query<FtCategory>
            (
                @"select
                    id as Id,
                    name as Name
                from ft_category"
            )
            .ToList();

        foreach (var category in result)
        {
            if (category.Name == "Конкурсные рассказы")
            {
                category.Name = "Конкурсные произведения";
            }
        }
        
        return result;
    }
    
    private async Task<Guid> AddTextToArkumidaAsync(TextDto textDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}Texts/Create", new CreateTextRequest() { Text = textDto });
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Failed to import text: { textDto.Title }");
            Console.WriteLine(response.StatusCode);
            
            throw new InvalidOperationException();
        }
            
        var responseData = JsonSerializer.Deserialize<CreateTextResponse>(await response.Content.ReadAsStringAsync());

        return responseData.Text.Id;
    }

    private async Task<TextTagDto> GetTagFromArkumidaByNameAsync(string name)
    {
        var response = await _httpClient.GetAsync($"{MainImporter.BaseUrl}Tags/ByName?name={ name.Replace("/", "%2F") }");
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }

        var responseData = JsonSerializer.Deserialize<TextTagResponse>(await response.Content.ReadAsStringAsync());

        return responseData.Tag;
    }
    
    private List<FtTextFile> LoadTextFilesByText(MySqlConnection connection, int textId)
    {
        return connection.Query<FtTextFile>
            (
                @"select
                    id as Id,
                    fileId as TextId,
                    name as Name,
                    md5 as Hash,
                    subType as SubType
                from ft_objects_files
                where fileId = @textId",
                new { textId = textId }
            )
            .ToList();
    }

    private string GenerateTextFilePath(int textId, string hash, int subtype)
    {
        var result = hash;

        switch (subtype)
        {
            case 1:
                result += ".gif";
                break;
            
            case 2:
                result += ".jpg";
                break;
            
            case 3:
                result += ".png";
                break;
                
            default:
                throw new ArgumentException("Wrong subtype!", nameof(subtype));
        }

        return $@"{MainImporter.TextsDbRoot}/{textId}/{result}";
    }

    private string GetMimeTypeByFileSubtype(int subtype)
    {
        switch (subtype)
        {
            case 1:
                return "image/gif";

            case 2:
                return "image/jpeg";
            
            case 3:
                return "image/png";
                
            default:
                throw new ArgumentException("Wrong subtype!", nameof(subtype));
        }
    }
    
    private async Task AddFileToArkumidaTextAsync(Guid textId, string fileName, Guid fileId)
    {
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}Texts/AddFile", new AddFileToTextRequest() { TextId = textId, Name = fileName, FileId = fileId });
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
    }

    private async Task<CreatureDto> RegisterUserIfNotExistAsync(string login)
    {
        Console.WriteLine($"Trying to register a creature with login { login }...");
        
        var creature = await _usersImporter.FindCreatureByLogin(login);
        if (creature == null)
        {
            await _usersImporter.RegisterUserAsync(new RegistrationDataDto() { Login = login, Email = string.Empty, Password = _usersImporter.GeneratePassword()} );
            creature = await _usersImporter.FindCreatureByLogin(login);
        }

        return creature;
    }
}