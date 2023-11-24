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

namespace webapi.OpenSearch.Models;

/// <summary>
/// Text, indexable to OpenSearch
/// </summary>
public class IndexableText : IIndexableEntity
{
    public static string IndexName => "texts";
    
    /// <summary>
    /// Creature ID
    /// </summary>
    public Guid DbId { get; set; }

    /// <summary>
    /// When text was updated last time
    /// </summary>
    public DateTime LastUpdateTime { get; set; }
    
    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Raw text content (without title, tags and so on - to avoid accidental search on it)
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Authors DB IDs
    /// </summary>
    public List<Guid> AuthorsDbIds { get; set; }

    /// <summary>
    /// Translators DB IDs (may be empty)
    /// </summary>
    public List<Guid> TranslatorsDbIds { get; set; }

    /// <summary>
    /// Publisher DB ID
    /// </summary>
    public Guid PublisherDbId { get; set; }

    /// <summary>
    /// Tags DB IDs
    /// </summary>
    public List<Guid> TagsDbIds { get; set; }
}