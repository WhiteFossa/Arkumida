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

using webapi.Dao.Models;
using webapi.Dao.Models.Enums;

namespace webapi.Dao.Abstract;

/// <summary>
/// DAO to work with tags
/// </summary>
public interface ITagsDao
{
    #region Create / update

    /// <summary>
    /// Create new tag
    /// </summary>
    Task CreateTagAsync(TagDbo tag);

    #endregion
    
    #region Get

    /// <summary>
    /// Get all tags (filtering by subtype if needed). If subtype == null, then no filters applied
    /// </summary>
    Task<IReadOnlyCollection<TagDbo>> GetTagsAsync(TagSubtype? subtype = null);

    /// <summary>
    /// Get tags, representing categories (ordered by CategoryOrder)
    /// </summary>
    Task<IReadOnlyCollection<TagDbo>> GetCategoryTagsAsync();

    /// <summary>
    /// Get tag by ID. If tag is not found - throws an exception
    /// </summary>
    Task<TagDbo> GetTagByIdAsync(Guid id);
    
    /// <summary>
    /// Get tag by name. Probably will be used only for import from old FT
    /// </summary>
    Task<TagDbo> GetTagByNameAsync(string name);
    
    /// <summary>
    /// Get popularity for given tags
    /// </summary>
    Task<Dictionary<Guid, int>> GetTagsPopularity(IReadOnlyCollection<Guid> tagsIds);

    /// <summary>
    /// Get texts count from the most popular tag. HIDDEN TAGS AREN'T COUNTED
    /// </summary>
    /// <returns></returns>
    Task<int> GetMaxTextsCountAsync();

    /// <summary>
    /// How much category tags we have in given tags collection?
    /// </summary>
    Task<int> CountCategoryTagsAsync(IReadOnlyCollection<Guid> tagsIds);
    
    /// <summary>
    /// How much text type tags we have in given tags collection?
    /// </summary>
    Task<int> CountTextTypeTagsAsync(IReadOnlyCollection<Guid> tagsIds);

    #endregion
}