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

using Microsoft.EntityFrameworkCore;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Dao.Models.Enums;

namespace webapi.Dao.Implementations;

public class TagsDao : ITagsDao
{
    private readonly MainDbContext _dbContext;

    /// <summary>
    /// If tag have on of those meanings than tag is text type tag
    /// </summary>
    private readonly TagMeaning[] TextTypeTagsMeanings = new[] { TagMeaning.TypeStories, TagMeaning.TypeNovels, TagMeaning.TypePoetry, TagMeaning.TypeComics }; 

    public TagsDao
    (
        MainDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateTagAsync(TagDbo tag)
    {
        _ = tag ?? throw new ArgumentNullException(nameof(tag), "Tag must not be null.");
        
        await _dbContext
            .Tags
            .AddAsync(tag);
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to insert 1 row, actually inserted { affected } rows!");
        }
    }

    public async Task<IReadOnlyCollection<TagDbo>> GetTagsAsync(TagSubtype? subtype = null)
    {
        IQueryable<TagDbo> tags = _dbContext
            .Tags;

        if (subtype.HasValue)
        {
            tags = tags
                .Where(t => t.Subtype == subtype.Value);
        }

        return await tags
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<TagDbo>> GetCategoryTagsAsync()
    {
        return await _dbContext
            .Tags
            .Where(t => t.IsCategory)
            .OrderBy(t => t.CategoryOrder)
            .ToListAsync();
    }

    public async Task<TagDbo> GetTagByIdAsync(Guid id)
    {
        return await _dbContext
            .Tags
            .SingleAsync(t => t.Id == id);
    }

    public async Task<TagDbo> GetTagByNameAsync(string name)
    {
        return await _dbContext
            .Tags
            .SingleAsync(t => t.Name.Equals(name));
    }

    public async Task<Dictionary<Guid, int>> GetTagsPopularity(IReadOnlyCollection<Guid> tagsIds)
    {
        return await _dbContext
            .Tags
            .Where(t => tagsIds.Contains(t.Id))
            .Select(t => new { Id = t.Id, Count = _dbContext.Texts.Count(tx => tx.Tags.Contains(t)) })
            .ToDictionaryAsync(t => t.Id, t => t.Count);
    }

    public async Task<int> GetMaxTextsCountAsync()
    {
        return await _dbContext
            .Tags
            .Where(t => !t.IsHidden)
            .Include(t => t.Texts)
            .MaxAsync(t => t.Texts.Count);
    }

    public async Task<int> CountCategoryTagsAsync(IReadOnlyCollection<Guid> tagsIds)
    {
        _ = tagsIds ?? throw new ArgumentNullException(nameof(tagsIds), "Tags list mustn't be null.");
        
        return await _dbContext
            .Tags
            .Where(t => t.IsCategory)
            .Where(t => tagsIds.Contains(t.Id))
            .CountAsync();
    }

    public async Task<int> CountTextTypeTagsAsync(IReadOnlyCollection<Guid> tagsIds)
    {
        _ = tagsIds ?? throw new ArgumentNullException(nameof(tagsIds), "Tags list mustn't be null.");
        
        return await _dbContext
            .Tags
            .Where(t => TextTypeTagsMeanings.Contains(t.Meaning))
            .Where(t => tagsIds.Contains(t.Id))
            .CountAsync();
    }
}