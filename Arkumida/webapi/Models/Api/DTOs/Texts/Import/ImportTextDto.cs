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
using webapi.Models.Api.DTOs.Creatures;
using webapi.Models.Creatures;

namespace webapi.Models.Api.DTOs.Texts.Import;

/// <summary>
/// Simplified DTO for texts import
/// </summary>
public class ImportTextDto
{
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
    /// Text tags IDs
    /// </summary>
    [JsonPropertyName("tags")]
    public IList<Guid> TagsIds { get; set; }
    
    /// <summary>
    /// If true, then text is not complete yet
    /// </summary>
    [JsonPropertyName("isIncomplete")]
    public bool IsIncomplete { get; set; }

    /// <summary>
    /// Text authors IDs
    /// </summary>
    [JsonPropertyName("authorsIds")]
    public IList<Guid> AuthorsIds { get; set; }
    
    /// <summary>
    /// Text translators IDs
    /// </summary>
    [JsonPropertyName("translatorsIds")]
    public IList<Guid> TranslatorsIds { get; set; }
    
    /// <summary>
    /// Text publisher ID
    /// </summary>
    [JsonPropertyName("publisherId")]
    public Guid PublisherId { get; set; }
    
    /// <summary>
    /// Convert to a new text
    /// </summary>
    public Text ToText()
    {
        return new Text()
        {
            Id = Guid.Empty, // This is new text
            CreateTime = CreateTime,
            LastUpdateTime = LastUpdateTime,
            Title = Title,
            Description = Description,
            Pages = Pages.Select(p => p.ToTextPage()).ToList(),
            Tags = TagsIds.Select(tid => new Tag() { Id = tid}).ToList(),
            IsIncomplete = IsIncomplete,
            Authors = AuthorsIds
                .Select
                (
                    aid
                    =>
                    new CreatureWithProfile
                    (
                        aid,
                        string.Empty, 
                        string.Empty,
                        false,
                        string.Empty,
                        string.Empty,
                        new List<Avatar>(),
                        null,
                        String.Empty
                    )
                )
                .ToList(),
            Translators = TranslatorsIds.Select
                (
                    aid
                        =>
                        new CreatureWithProfile
                        (
                            aid,
                            string.Empty, 
                            string.Empty,
                            false,
                            string.Empty,
                            string.Empty,
                            new List<Avatar>(),
                            null,
                            String.Empty
                        )
                )
                .ToList(),
            Publisher = new CreatureWithProfile
            (
                PublisherId,
                string.Empty, 
                string.Empty,
                false,
                string.Empty,
                string.Empty,
                new List<Avatar>(),
                null,
                String.Empty
            )
        };
    }
}