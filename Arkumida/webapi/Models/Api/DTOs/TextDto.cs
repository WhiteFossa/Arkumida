#region License
// Arkumida - Furtails.pw next generation backend
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

using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Text DTO
/// </summary>
public class TextDto
{
    /// <summary>
    /// Text ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// When text was created
    /// </summary>
    [JsonPropertyName("createTime")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// When text was updated last time
    /// </summary>
    [JsonPropertyName("lastUpdateTime")]
    public DateTime LastUpdateTime { get; set; }

    /// <summary>
    /// Text title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    /// Text description (for text info)
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// Text pages
    /// </summary>
    [JsonPropertyName("pages")]
    public IList<TextPageDto> Pages { get; set; }

    /// <summary>
    /// Text tags
    /// </summary>
    [JsonPropertyName("tags")]
    public IList<TagDto> Tags { get; set; }
    
    /// <summary>
    /// If true, then text is not complete yet
    /// </summary>
    [JsonPropertyName("isIncomplete")]
    public bool IsIncomplete { get; set; }

    /// <summary>
    /// Text authors
    /// </summary>
    [JsonPropertyName("authors")]
    public IList<CreatureWithProfileDto> Authors { get; set; }
    
    /// <summary>
    /// Text translators
    /// </summary>
    [JsonPropertyName("translators")]
    public IList<CreatureWithProfileDto> Translators { get; set; }
    
    /// <summary>
    /// Text publisher
    /// </summary>
    [JsonPropertyName("publisher")]
    public CreatureWithProfileDto Publisher { get; set; }

    public TextDto()
    {
        
    }

    public TextDto
    (
        Guid id,
        DateTime createTime,
        DateTime lastUpdateTime,
        string title,
        string description,
        IReadOnlyCollection<TextPageDto> pages,
        IReadOnlyCollection<TagDto> tags,
        bool isIncomplete,
        IList<CreatureWithProfileDto> authors,
        IList<CreatureWithProfileDto> translators,
        CreatureWithProfileDto publisher
    )
    {
        Id = id;
        CreateTime = createTime;
        LastUpdateTime = lastUpdateTime;

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title must be populated.", nameof(title));
        }
        Title = title;
        
        Description = description; // May be empty
        Pages = (pages ?? throw new ArgumentNullException(nameof(pages), "Pages mustn't be null.")).ToList();
        
        Tags = (tags ?? throw new ArgumentNullException(nameof(tags), "Tags mustn't be null.")).ToList();

        IsIncomplete = isIncomplete;

        Authors = authors ?? throw new ArgumentNullException(nameof(authors), "Authors must be specified.");
        if (!Authors.Any())
        {
            throw new ArgumentException("At least one author is required!", nameof(authors));
        }
        
        // We may have empty translators list, but still not null
        Translators = translators ?? throw new ArgumentNullException(nameof(translators), "Translators must be specified.");
        
        Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher), "Publisher must be specified.");
    }

    public Text ToText()
    {
        return new Text()
        {
            Id = Id,
            CreateTime = CreateTime,
            LastUpdateTime = LastUpdateTime,
            Title = Title,
            Description = Description,
            Pages = Pages.Select(p => p.ToTextPage()).ToList(),
            Tags = Tags.Select(t => t.ToTag()).ToList(),
            IsIncomplete = IsIncomplete,
            Authors = Authors.Select(a => a.ToCreatureWithProfile()).ToList(),
            Translators = Translators.Select(t => t.ToCreatureWithProfile()).ToList(),
            Publisher = Publisher.ToCreatureWithProfile()
        };
    }
}