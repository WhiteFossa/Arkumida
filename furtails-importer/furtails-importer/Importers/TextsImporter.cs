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

    public TextsImporter(MySqlConnection connection, HttpClient httpClient)
    {
        _connection = connection;
        _httpClient = httpClient;
    }

    public async Task Import()
    {
        var categories = LoadCategories();
        
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

        var textNumber = 0;
        foreach (var text in texts)
        {
            if (text.IsDeleted != 0)
            {
                continue; // Deleted text
            }

            if (text.PublishStatus != 2)
            {
                continue; // Not published yet
            }
            
            var textModel = new Text();

            // We need files metadata early to process comics
            var textFilesMetadata = LoadTextFilesByText(text.Id);
            
            if (text.Type == 3) // Editing
            {
                var sections = LoadWIPTextSections(text.Id);
                
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
                
                var parts = LoadTranslationParts(text.Id);

                foreach (var part in parts)
                {
                    var sectionModel = new TextSection()
                    {
                        OriginalText = TextsHelper.FixupText(part.OriginalText),
                        Variants = new List<TextSectionVariant>()
                    };
                    
                    // Loading variants for each section
                    var variants = LoadTranslationVariants(part.Id);
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
                var links = LoadLinksByText(text.Id);

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
            var textTagsRelations = LoadTextsToTagsRelations(text.Id);
            foreach (var tagRelation in textTagsRelations)
            {
                var tagName = LoadTagById(tagRelation.TagId).Name;
                
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
                
                IsIncomplete = text.IsNotFinished
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

                var uploadedFile = await UploadFileToArkumidaAsync(fileMetadata.Name, mimeType, content);
                
                // Now just attach file to text
                await AddFileToArkumidaTextAsync(arkumidaTextId, fileMetadata.Name, uploadedFile.FileInfo.Id);
            }
            
            Console.WriteLine($"Text { textNumber }");
            
            textNumber++;
        }
    }

    private string LoadTextById(int id)
    {
        return File.ReadAllText($@"{MainImporter.TextsDbRoot}/{id}/TEXT");
    }

    private List<FtRawTextSection> LoadWIPTextSections(int textId)
    {
        return _connection.Query<FtRawTextSection>
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

    private List<FtTranslateTextPart> LoadTranslationParts(int textId)
    {
        return _connection.Query<FtTranslateTextPart>
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

    private List<FtTranslateTextVariant> LoadTranslationVariants(int partId)
    {
        return _connection.Query<FtTranslateTextVariant>
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

    private List<FtTextLink> LoadLinksByText(int textId)
    {
        return _connection.Query<FtTextLink>
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

    private List<FtTextTag> LoadTextsToTagsRelations(int textId)
    {
        return _connection.Query<FtTextTag>
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

    private FtTag LoadTagById(int tagId)
    {
        return _connection.Query<FtTag>
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
    
    private List<FtCategory> LoadCategories()
    {
        var result = _connection.Query<FtCategory>
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
    
    private List<FtTextFile> LoadTextFilesByText(int textId)
    {
        return _connection.Query<FtTextFile>
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
    
    private async Task<UploadFileResponse> UploadFileToArkumidaAsync(string filename, string mimeType, byte[] content)
    {
        var streamContent = new StreamContent(new MemoryStream(content));
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
        
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{MainImporter.BaseUrl}Files/Upload");
        using var requestContent = new MultipartFormDataContent
        {
            {
                streamContent,
                "file",
                filename
            }
        };
        
        request.Content = requestContent;
        
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
        
        return JsonSerializer.Deserialize<UploadFileResponse>(await response.Content.ReadAsStringAsync());
    }
    
    private async Task AddFileToArkumidaTextAsync(Guid textId, string fileName, Guid fileId)
    {
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}Texts/AddFile", new AddFileToTextRequest() { TextId = textId, Name = fileName, FileId = fileId });
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
    }
}