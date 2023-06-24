using System.Collections.ObjectModel;
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

            if (text.Type == 3) // Editing
            {
                var sections = LoadWIPTextSections(text.Id);
                
                // Many sections, each have one variant
                textModel.Sections = new List<TextSection>();
                
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
                    
                    textModel.Sections.Add(sectionModel);
                }
            }
            else if (text.Type == 5) // Translation
            {
                // Many sections, each have many variants
                textModel.Sections = new List<TextSection>();
                
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
                    
                    textModel.Sections.Add(sectionModel);
                }
            }
            else if (text.Type == 1) // Just a text
            {
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

                textModel.Sections = new List<TextSection>() { sectionModel };
            }
            else if (text.Type == 2) // Links set
            {
                textModel.Sections = new List<TextSection>();
                
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
                    
                    textModel.Sections.Add(sectionModel);
                }
            }
            else if (text.Type == 4) // Comics
            {
                // Not supported yet
                continue;
            }
            
            // Loading tags
            var textTagsRelations = LoadTextsToTagsRelations(text.Id);

            var arkumidaTagsIds = new List<Guid>();
            foreach (var tagRelation in textTagsRelations)
            {
                var tagName = LoadTagById(tagRelation.TagId).Name;

                arkumidaTagsIds.Add((await GetTagFromArkumidaByNameAsync(tagName)).Id);
            }
            
            // Adding category tags
            var textCategory = categories.Single(c => c.Id == text.CategoryId);

            arkumidaTagsIds.Add((await GetTagFromArkumidaByNameAsync(textCategory.Name)).Id);
            if (textCategory.Name != "Рассказы" && textCategory.Name != "Повести и Романы" && textCategory.Name != "Стихи" && textCategory.Name != "Комиксы")
            {
                // No size category, in this case we need to add size category manually
                arkumidaTagsIds.Add((await GetTagFromArkumidaByNameAsync("Рассказы")).Id); // TODO: In future, we can try to detect correct size category by analyzing text size
            }

            // Now we have text model ready
            var textToCreate = new TextDto()
            {
                Id = Guid.Empty,
                CreateTime = text.CreateTime.ToUniversalTime(),
                LastUpdateTime = (text.UpdateTime.HasValue ? text.UpdateTime.Value : text.CreateTime).ToUniversalTime(),
                Title = text.Title,
                Description = text.Description,
                Sections = new Collection<TextSectionDto>(),
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
                }).ToList()
            };

            var sectionOrder = 0;
            foreach (var section in textModel.Sections)
            {
                var sectionToCreate = new TextSectionDto()
                {
                    Id = Guid.Empty,
                    OriginalText = section.OriginalText,
                    Order = sectionOrder,
                    Variants = new List<TextSectionVariantDto>()
                };
                
                textToCreate
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

            await AddTextToArkumidaAsync(textToCreate);
            
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
    
    private async Task<Guid> AddTextSectionToArkumidaAsync(TextSectionDto sectionDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}TextsSections/Create", new CreateTextSectionRequest() { Section = sectionDto });
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
            
        var responseData = JsonSerializer.Deserialize<CreateTextSectionResponse>(await response.Content.ReadAsStringAsync());

        return responseData.Section.Id;
    }
    
    private async Task AttachSectionToTextAsync(Guid textId, Guid sectionId)
    {
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}Texts/AddSection", new AddSectionToTextRequest() { TextId = textId, SectionId = sectionId });
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
    }
    
    private async Task<Guid> AddTextVariantToArkumidaAsync(TextSectionVariantDto variantDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}TextsSectionsVariants/Create", new CreateTextSectionVariantRequest() { Variant = variantDto });
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
            
        var responseData = JsonSerializer.Deserialize<CreateTextSectionVariantResponse>(await response.Content.ReadAsStringAsync());

        return responseData.Variant.Id;
    }
    
    private async Task AttachVariantToSectionAsync(Guid sectionId, Guid variantId)
    {
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}TextsSections/AddVariant", new AddVariantToSectionRequest() { SectionId = sectionId, VariantId = variantId });
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
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
}