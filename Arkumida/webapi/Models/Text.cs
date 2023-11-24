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

using webapi.Models.Api.DTOs;
using webapi.Services.Abstract;

namespace webapi.Models;

/// <summary>
/// One text
/// TODO: Add constructor, important
/// </summary>
public class Text
{
    /// <summary>
    /// Text ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// When text was created
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// When text was updated last time
    /// </summary>
    public DateTime LastUpdateTime { get; set; }

    /// <summary>
    /// Text title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Text description (for text info)
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Text pages
    /// </summary>
    public IList<TextPage> Pages { get; set; }
    
    /// <summary>
    /// Tags
    /// </summary>
    public IList<Tag> Tags { get; set; }
    
    /// <summary>
    /// If true, then text is not complete yet
    /// </summary>
    public bool IsIncomplete { get; set; }
    
    /// <summary>
    /// Files, attached to text
    /// </summary>
    public IList<TextFile> TextFiles { get; set; }
    
    /// <summary>
    /// Text authors
    /// </summary>
    public IList<CreatureWithProfile> Authors { get; set; }

    /// <summary>
    /// Text translators
    /// </summary>
    public IList<CreatureWithProfile> Translators { get; set; }

    /// <summary>
    /// Text publisher
    /// </summary>
    public CreatureWithProfile Publisher { get; set; }

    public TextDto ToDto(ITextUtilsService textUtilsService)
    {
        return new TextDto
        (
            Id,
            CreateTime,
            LastUpdateTime,
            Title,
            Description,
            Pages.Select(p => p.ToDto(this.TextFiles.ToList(), textUtilsService)).ToList(),
            Tags.Select(t => t.ToTagDto()).ToList(),
            IsIncomplete,
            Authors.Select(a => a.ToDto()).ToList(),
            Translators.Select(t => t.ToDto()).ToList(),
            Publisher.ToDto()
        );
    }
}