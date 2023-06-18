using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using furtails_importer.Dbos;
using furtails_importer.Models;
using furtails_importer.WebClientStuff.Dtos;
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

        var textOrder = 0;
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
                        Content = section.SectionRus,
                        CreationTime = section.SaveDate
                    };

                    var sectionModel = new TextSection()
                    {
                        OriginalText = section.SectionEng,
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
                        OriginalText = part.OriginalText,
                        Variants = new List<TextSectionVariant>()
                    };
                    
                    // Loading variants for each section
                    var variants = LoadTranslationVariants(part.Id);
                    foreach (var variant in variants)
                    {
                        sectionModel.Variants.Add(new TextSectionVariant()
                        {
                            Content = variant.Text,
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
                    Content = LoadTextById(text.Id),
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
            
            // Now we have text model ready

            // Text
            var textId = await AddTextToArkumidaAsync(new TextDto()
            {
                Id = Guid.Empty,
                CreateTime = text.CreateTime.ToUniversalTime(),
                LastUpdateTime = (text.UpdateTime.HasValue ? text.UpdateTime.Value : text.CreateTime).ToUniversalTime(),
                Title = text.Title,
                Description = text.Description,
                Sections = new Collection<TextSectionDto>()
            });

            // Creating its sections
            var sectionOrder = 0;
            foreach (var section in textModel.Sections)
            {
                var sectionId = await AddTextSectionToArkumidaAsync(new TextSectionDto()
                {
                    Id = Guid.Empty,
                    OriginalText = section.OriginalText,
                    Order = sectionOrder,
                    Variants = new List<TextSectionVariantDto>()
                });

                // Adding section to text
                await AttachSectionToTextAsync(textId, sectionId);
                
                // Adding variants here
                
                sectionOrder ++;
                Console.WriteLine($"Section { sectionOrder }/{ textModel.Sections.Count }, Text { textOrder }/{ texts.Count }");
            }

            textOrder++;
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
    
    private async Task<Guid> AddTextToArkumidaAsync(TextDto textDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}Texts/Create", new CreateTextRequest() { Text = textDto });
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
            
        var responseData = JsonSerializer.Deserialize<CreateTextResponse>(await response.Content.ReadAsStringAsync());

        //Console.WriteLine($"Text { responseData.Text.Title } successfully imported.");

        return responseData.Text.Id;
    }
    
    private async Task<Guid> AddTextSectionToArkumidaAsync(TextSectionDto sectionDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}TextsSections/Create", new CreateTextSectionRequest() { Section = sectionDto});
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
            
        var responseData = JsonSerializer.Deserialize<CreateTextSectionResponse>(await response.Content.ReadAsStringAsync());

        //Console.WriteLine($"Section { responseData.Section.Id } successfully imported.");

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
}