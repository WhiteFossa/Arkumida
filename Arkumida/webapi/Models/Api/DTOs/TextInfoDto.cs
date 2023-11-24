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

public class TextInfoDto : IdedEntityDto
{
    /// <summary>
    /// Text authors
    /// </summary>
    [JsonPropertyName("authors")]
    public IReadOnlyCollection<CreatureWithProfileDto> Authors { get; private set; }
    
    /// <summary>
    /// Text translators
    /// </summary>
    [JsonPropertyName("translators")]
    public IReadOnlyCollection<CreatureWithProfileDto> Translators { get; private set; }
    
    /// <summary>
    /// Text publisher
    /// </summary>
    [JsonPropertyName("publisher")]
    public CreatureWithProfileDto Publisher { get; private set; }

    /// <summary>
    /// Text title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; private set; }
    
    /// <summary>
    /// Text add time
    /// </summary>
    [JsonPropertyName("addTime")]
    public DateTime AddTime { get; private set; }

    /// <summary>
    /// Views count
    /// </summary>
    [JsonPropertyName("viewsCount")]
    public long ViewsCount { get; private set; }
    
    /// <summary>
    /// Comments count
    /// </summary>
    [JsonPropertyName("commentsCount")]
    public int CommentsCount { get; private set; }

    /// <summary>
    /// Text tags (including category tags)
    /// </summary>
    [JsonPropertyName("tags")]
    public IReadOnlyCollection<TextTagDto> Tags { get; private set; }
    
    /// <summary>
    /// Additional left icons
    /// </summary>
    [JsonPropertyName("leftIcons")]
    public IReadOnlyCollection<TextIconDto> LeftIcons { get; private set; }
    
    /// <summary>
    /// Additional right icons
    /// </summary>
    [JsonPropertyName("rightIcons")]
    public IReadOnlyCollection<TextIconDto> RightIcons { get; private set; }

    /// <summary>
    /// Short text description
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; private set; }

    /// <summary>
    /// Text size in bytes
    /// </summary>
    [JsonPropertyName("sizeInBytes")]
    public int SizeInBytes { get; private set; }

    /// <summary>
    /// Text size in pages (initially made for comics)
    /// </summary>
    [JsonPropertyName("sizeInPages")]
    public int SizeInPages { get; private set; }
    
    /// <summary>
    /// If true, then text is not complete yet
    /// </summary>
    [JsonPropertyName("isIncomplete")]
    public bool IsIncomplete { get; set; }

    public TextInfoDto
    (
        Guid id,
        string furryReadableId,
        IReadOnlyCollection<CreatureWithProfileDto> authors,
        IReadOnlyCollection<CreatureWithProfileDto> translators,
        CreatureWithProfileDto publisher,
        string title,
        DateTime addTime,
        long viewsCount,
        int commentsCount,
        IReadOnlyCollection<TextTagDto> tags,
        IReadOnlyCollection<TextIconDto> leftIcons,
        IReadOnlyCollection<TextIconDto> rightIcons,
        string description,
        int sizeInBytes,
        int sizeInPages,
        bool isIncomplete
    ) : base(id, furryReadableId)
    {
        Authors = authors ?? throw new ArgumentNullException(nameof(authors), "Authors must not be null");
        if (!Authors.Any())
        {
            throw new ArgumentException("At least one author is required!", nameof(authors));
        }
        
        Translators = translators ?? throw new ArgumentNullException(nameof(translators), "Translators must not be null");
        Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher), "Publisher must not be null");

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title must be populated.", nameof(title));
        }
        Title = title;
        
        AddTime = addTime;

        if (viewsCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(viewsCount), "Views count must be positive");
        }
        ViewsCount = viewsCount;
        
        if (commentsCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(commentsCount), "Comments count must be positive");
        }
        CommentsCount = commentsCount;

        Tags = tags ?? throw new ArgumentNullException(nameof(tags), "Tags must not be null");
        LeftIcons = leftIcons ?? throw new ArgumentNullException(nameof(leftIcons), "Left icons must not be null");
        RightIcons = rightIcons ?? throw new ArgumentNullException(nameof(rightIcons), "Right icons must not be null");
        Description = description; // Unfortunately there is some stories in old FT DB, where descriptions are empty
        
        if (sizeInBytes < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sizeInBytes), "Size in bytes must be positive."); // 0 is OK for comics
        }
        SizeInBytes = sizeInBytes;
        
        if (sizeInPages < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sizeInPages), "Size in pages must be positive."); // 0 is OK for non-comics
        }
        SizeInPages = sizeInPages;

        IsIncomplete = isIncomplete;
    }
}