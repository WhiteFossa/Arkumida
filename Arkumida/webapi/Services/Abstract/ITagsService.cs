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

using webapi.Dao.Models.Enums;
using webapi.Models;

namespace webapi.Services.Abstract;

public interface ITagsService
{
    /// <summary>
    /// Returns all tags, which are categories.
    /// </summary>
    Task<IReadOnlyCollection<Tag>> GetCategoriesTagsAsync();

    /// <summary>
    /// Get all existing tags, applying subtype filter if needed
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<Tag>> GetAllTagsAsync(TagSubtype? subtype = null);
    
    /// <summary>
    /// Get tag by ID
    /// </summary>
    Task<Tag> GetTagByIdAsync(Guid id);
    
    /// <summary>
    /// Get tag by name
    /// </summary>
    Task<Tag> GetTagByNameAsync(string name);

    /// <summary>
    /// Create new tag
    /// </summary>
    Task CreateTagAsync(Tag tag);

    /// <summary>
    /// Order tags (for displaying in text info, for example)
    /// </summary>
    IReadOnlyCollection<Tag> OrderTags(IEnumerable<Tag> tags);
    
    /// <summary>
    /// How much category tags we have in given tags collection?
    /// </summary>
    Task<int> CountCategoryTagsAsync(IReadOnlyCollection<Guid> tagsIds);
    
    /// <summary>
    /// How much text type tags we have in given tags collection?
    /// </summary>
    Task<int> CountTextTypeTagsAsync(IReadOnlyCollection<Guid> tagsIds);
}