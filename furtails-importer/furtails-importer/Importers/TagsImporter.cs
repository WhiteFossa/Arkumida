using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using furtails_importer.Dbos;
using furtails_importer.WebClientStuff.Dtos;
using furtails_importer.WebClientStuff.Enums;
using furtails_importer.WebClientStuff.Requests;
using furtails_importer.WebClientStuff.Responses;
using MySqlConnector;

namespace furtails_importer.Importers;

public class TagsImporter
{
    private readonly MySqlConnection _connection;
    private readonly HttpClient _httpClient;

    public TagsImporter(MySqlConnection connection, HttpClient httpClient)
    {
        _connection = connection;
        _httpClient = httpClient;
    }

    public async Task Import()
    {
        var tags = _connection.Query<FtTag>
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
                from ft_tags"
            )
            .ToList();
        
        foreach (var tag in tags)
        {
            var category = DetectCategory(tag);
            
            var tagDto = new TagDto()
            {
                Id = Guid.NewGuid(),
                FurryReadableId = tag.Id.ToString(),
                Subtype = GroupIdToTagSubtype(tag.GroupId),
                Name = tag.Name,
                IsCategory = category.IsCategory,
                CategoryOrder = category.Order,
                CategoryTagType = category.Type,
                IsHidden = false,
                Meaning = TagMeaning.Unspecified // Ordinary tags
            };

            await AddTagToArkumidaAsync(tagDto);
        }
        
        // Adding special categories tags
        var stories = new TagDto()
        {
            Id = Guid.NewGuid(),
            FurryReadableId = "stories",
            Subtype = TagSubtype.Category,
            Name = "Рассказы",
            IsCategory = true,
            CategoryOrder = 700,
            CategoryTagType = CategoryTagType.Normal,
            IsHidden = true,
            Meaning = TagMeaning.TypeStories
        };
        await AddTagToArkumidaAsync(stories);

        
        var novels = new TagDto()
        {
            Id = Guid.NewGuid(),
            FurryReadableId = "novels",
            Subtype = TagSubtype.Category,
            Name = "Повести и Романы",
            IsCategory = true,
            CategoryOrder = 800,
            CategoryTagType = CategoryTagType.Normal,
            IsHidden = true,
            Meaning = TagMeaning.TypeNovels
        };
        await AddTagToArkumidaAsync(novels);
        
        var poetry = new TagDto()
        {
            Id = Guid.NewGuid(),
            FurryReadableId = "poetry",
            Subtype = TagSubtype.Category,
            Name = "Стихи",
            IsCategory = true,
            CategoryOrder = 900,
            CategoryTagType = CategoryTagType.Normal,
            IsHidden = true,
            Meaning = TagMeaning.TypePoetry
        };
        await AddTagToArkumidaAsync(poetry);
        
        var snurf = new TagDto()
        {
            Id = Guid.NewGuid(),
            FurryReadableId = "snurf",
            Subtype = TagSubtype.Category,
            Name = "Логово Снарфа",
            IsCategory = true,
            CategoryOrder = 1000,
            CategoryTagType = CategoryTagType.Snuff,
            IsHidden = true,
            Meaning = TagMeaning.SpecTypeSnuff
        };
        await AddTagToArkumidaAsync(snurf);
        
        var hackwrench = new TagDto()
        {
            Id = Guid.NewGuid(),
            FurryReadableId = "hackwrench",
            Subtype = TagSubtype.Category,
            Name = "Мастерская Гайки",
            IsCategory = true,
            CategoryOrder = 1100,
            CategoryTagType = CategoryTagType.Normal,
            IsHidden = true,
            Meaning = TagMeaning.Unspecified
        };
        await AddTagToArkumidaAsync(hackwrench);
        
        var metamor = new TagDto()
        {
            Id = Guid.NewGuid(),
            FurryReadableId = "metamor",
            Subtype = TagSubtype.Category,
            Name = "Цитадель Метамор",
            IsCategory = true,
            CategoryOrder = 1200,
            CategoryTagType = CategoryTagType.Normal,
            IsHidden = true,
            Meaning = TagMeaning.Unspecified
        };
        await AddTagToArkumidaAsync(metamor);
        
        var sandbox = new TagDto()
        {
            Id = Guid.NewGuid(),
            FurryReadableId = "sandbox",
            Subtype = TagSubtype.Category,
            Name = "Песочница",
            IsCategory = true,
            CategoryOrder = 1300,
            CategoryTagType = CategoryTagType.Sandbox,
            IsHidden = true,
            Meaning = TagMeaning.SpecTypeSandbox
        };
        await AddTagToArkumidaAsync(sandbox);
        
        var comics = new TagDto()
        {
            Id = Guid.NewGuid(),
            FurryReadableId = "comics",
            Subtype = TagSubtype.Category,
            Name = "Комиксы",
            IsCategory = true,
            CategoryOrder = 1400,
            CategoryTagType = CategoryTagType.Normal,
            IsHidden = true,
            Meaning = TagMeaning.TypeComics
        };
        await AddTagToArkumidaAsync(comics);
        
        var contest = new TagDto()
        {
            Id = Guid.NewGuid(),
            FurryReadableId = "contest",
            Subtype = TagSubtype.Category,
            Name = "Конкурсные рассказы",
            IsCategory = true,
            CategoryOrder = 1500,
            CategoryTagType = CategoryTagType.Contest,
            IsHidden = true,
            Meaning = TagMeaning.SpecTypeContest
        };
        await AddTagToArkumidaAsync(contest);
        
        var chakona = new TagDto()
        {
            Id = Guid.NewGuid(),
            FurryReadableId = "chakona",
            Subtype = TagSubtype.Category,
            Name = "Chakona",
            IsCategory = true,
            CategoryOrder = 1600,
            CategoryTagType = CategoryTagType.Normal,
            IsHidden = true,
            Meaning = TagMeaning.Unspecified
        };
        await AddTagToArkumidaAsync(chakona);
        
        var series = new TagDto()
        {
            Id = Guid.NewGuid(),
            FurryReadableId = "series",
            Subtype = TagSubtype.Category,
            Name = "Серии",
            IsCategory = true,
            CategoryOrder = 1700,
            CategoryTagType = CategoryTagType.Normal,
            IsHidden = true,
            Meaning = TagMeaning.Unspecified
        };
        await AddTagToArkumidaAsync(series);
    }

    private async Task AddTagToArkumidaAsync(TagDto tagDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}Tags/Create", new CreateTagRequest() { Tag = tagDto });
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
            
        var responseData = JsonSerializer.Deserialize<CreateTagResponse>(await response.Content.ReadAsStringAsync());

        Console.WriteLine($"Tag { responseData.Tag.Name} successfully imported.");
    }

    private TagSubtype GroupIdToTagSubtype(int groupId)
    {
        switch (groupId)
        {
            case 1:
                return TagSubtype.Participants;

            case 2:
                return TagSubtype.Species;

            case 3:
                return TagSubtype.Setting;

            case 4:
                return TagSubtype.Actions;

            default:
                throw new InvalidOperationException();
        }
    }

    private class Category
    {
        public bool IsCategory { get; set; }
        
        public int Order { get; set; }

        public CategoryTagType Type { get; set; }
    }

    private Category DetectCategory(FtTag tag)
    {
        switch (tag.Name)
        {
            case "F":
                return new Category() { IsCategory = true, Order = 100, Type = CategoryTagType.Normal };
            
            case "F/F":
                return new Category() { IsCategory = true, Order = 200, Type = CategoryTagType.Normal };
            
            case "M":
                return new Category() { IsCategory = true, Order = 300, Type = CategoryTagType.Normal };
            
            case "M/F":
                return new Category() { IsCategory = true, Order = 400, Type = CategoryTagType.Normal };
            
            case "M/M":
                return new Category() { IsCategory = true, Order = 500, Type = CategoryTagType.Normal };
            
            case "NO YIFF":
                return new Category() { IsCategory = true, Order = 600, Type = CategoryTagType.Normal };

            default:
                return new Category() { IsCategory = false, Order = 0, Type = CategoryTagType.Normal };
        }
    }
}