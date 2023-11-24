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

using System.ComponentModel.DataAnnotations;

namespace webapi.Dao.Models;

public class TextDbo
{
    /// <summary>
    /// Text ID
    /// </summary>
    [Key]
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
    public IList<TextPageDbo> Pages { get; set; }

    /// <summary>
    /// Text's tags
    /// </summary>
    public IList<TagDbo> Tags { get; set; }

    /// <summary>
    /// If true, then text is not complete yet
    /// </summary>
    public bool IsIncomplete { get; set; }

    /// <summary>
    /// Files, attached to text
    /// </summary>
    public IList<TextFileDbo> TextFiles { get; set; }

    /// <summary>
    /// Text authors
    /// </summary>
    public IList<CreatureDbo> Authors { get; set; }

    /// <summary>
    /// Text translator
    /// </summary>
    public IList<CreatureDbo> Translators { get; set; }

    /// <summary>
    /// Text publisher
    /// </summary>
    public CreatureDbo Publisher { get; set; }

    /// <summary>
    /// Rendered texts (plaintext, PDF etc)
    /// </summary>
    public IList<RenderedTextDbo> RenderedTexts { get; set; }
}