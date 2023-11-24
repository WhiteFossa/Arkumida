#region License
// Furtails Importer - Importer from furtails.pw database to Arkumida
// Copyright (C) 2023  Earlybeasts
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

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

        var tagsToAdd = new List<TagDto>();
        
        foreach (var tag in tags)
        {
            if (tag.Name == "конкурс")
            {
                // We don't need the "contest" tag, because we have "contest" category tag
                continue;
            }
            
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
                Meaning = DetectOrdinaryTagMeaning(tag)
            };

            tagsToAdd.Add(tagDto);
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
        tagsToAdd.Add(stories);

        
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
        tagsToAdd.Add(novels);
        
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
        tagsToAdd.Add(poetry);
        
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
        tagsToAdd.Add(snurf);
        
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
        tagsToAdd.Add(hackwrench);
        
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
        tagsToAdd.Add(metamor);
        
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
        tagsToAdd.Add(sandbox);
        
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
        tagsToAdd.Add(comics);
        
        var contest = new TagDto()
        {
            Id = Guid.NewGuid(),
            FurryReadableId = "contest",
            Subtype = TagSubtype.Category,
            Name = "Конкурсные произведения",
            IsCategory = true,
            CategoryOrder = 1500,
            CategoryTagType = CategoryTagType.Contest,
            IsHidden = true,
            Meaning = TagMeaning.SpecTypeContest
        };
        tagsToAdd.Add(contest);
        
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
        tagsToAdd.Add(chakona);
        
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
        tagsToAdd.Add(series);
        
        var parallelismDegree = new ParallelOptions()
        {
            MaxDegreeOfParallelism = MainImporter.ParallelismDegree
        };
        
        await Parallel.ForEachAsync(tagsToAdd, parallelismDegree, async (tagToAdd, token) =>
        {
            await AddTagToArkumidaAsync(tagToAdd);
        });
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

    /// <summary>
    /// Detect meaning for non-special tag
    /// </summary>
    private TagMeaning DetectOrdinaryTagMeaning(FtTag tag)
    {
        if (tag.Name == "MLP")
        {
            return TagMeaning.MLP;
        }

        return TagMeaning.Unspecified;
    }
}