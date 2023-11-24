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

public class TextsDao : ITextsDao
{
    private readonly MainDbContext _dbContext;
    
    public TextsDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateTextAsync(TextDbo text)
    {
        _ = text ?? throw new ArgumentNullException(nameof(text), "Text must not be null.");
        
        _ = text.Authors ?? throw new ArgumentNullException(nameof(text.Authors), "Text authors must be specified!");
        if (!text.Authors.Any())
        {
            throw new ArgumentException("Text must have at least one author!", nameof(text.Authors));
        }
        
        // Text may have no translators, in this case text.Translators will be empty (but still not null!)
        _ = text.Translators ?? throw new ArgumentNullException(nameof(text.Translators), "Text translators must be specified");
        
        _ = text.Publisher ?? throw new ArgumentNullException(nameof(text.Publisher), "Text publisher must be specified!");
        
        // Loading tags
        if (text.Tags != null)
        {
            text.Tags = text
                .Tags
                .Select(tag => _dbContext.Tags.Single(t => t.Id == tag.Id))
                .ToList();
        }

        // Loading creatures profiles by their IDs
        text.Authors = await LoadCreaturesAsync(text.Authors.Select(ta => ta.Id).ToList());
        text.Translators = await LoadCreaturesAsync(text.Translators.Select(tt => tt.Id).ToList());
        text.Publisher = await LoadCreatureAsync(text.Publisher.Id);

        await _dbContext
            .Texts
            .AddAsync(text);
        
        await _dbContext.SaveChangesAsync();
    }

    private async Task<CreatureDbo> LoadCreatureAsync(Guid creatureId)
    {
        return await _dbContext
            .Users
            .SingleAsync(u => u.Id == creatureId);
    }

    private async Task<IList<CreatureDbo>> LoadCreaturesAsync(IReadOnlyCollection<Guid> creaturesIds)
    {
        var result = new List<CreatureDbo>();

        foreach (var creatureId in creaturesIds)
        {
            result.Add(await LoadCreatureAsync(creatureId));
        }

        return result;
    }

    public async Task<TextFileDbo> AddFileToTextAsync(Guid textId, string name, Guid fileId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Filename must not be empty!", nameof(name));
        }
        
        var file = _dbContext
            .Files
            .Single(f => f.Id == fileId);

        var textFileDbo = new TextFileDbo()
        {
            Name = name,
            File = file
        };

        var textDbo = await GetTextWithFilesByIdAsync(textId);
        textDbo.TextFiles.Add(textFileDbo);
        
        await _dbContext.SaveChangesAsync();

        return textFileDbo;
    }

    public async Task<IReadOnlyCollection<TextDbo>> GetTextsMetadataOrderedByUpdateTimeAsync(int skip, int take)
    {
        if (skip < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skip), "Skip must not be negative.");
        }

        if (take <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(take), "Take must be positive.");
        }

        return await _dbContext
            .Texts
            .Include(t => t.Tags)
            .Include(t => t.TextFiles)
            .Include(t => t.Authors)
            .Include(t => t.Translators)
            .Include(t => t.Publisher)
            .OrderByDescending(t => t.LastUpdateTime).Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<TextDbo>> GetTextsMetadataExternallyOrderedAsync(IReadOnlyCollection<Guid> orderedIds)
    {
        var unorderedResult = await _dbContext
            .Texts
            .Include(t => t.Tags)
            .Include(t => t.TextFiles)
            .Include(t => t.Authors)
            .Include(t => t.Translators)
            .Include(t => t.Publisher)
            .Where(t => orderedIds.Contains(t.Id))
            .ToListAsync();

        return orderedIds
            .Select(oi => unorderedResult.Single(tm => tm.Id == oi))
            .ToList();
    }

    public async Task<Dictionary<Guid, TextDbo>> GetTextsMetadataByIdsAsync(IReadOnlyCollection<Guid> textsIds)
    {
        return await _dbContext
            .Texts
            .Include(t => t.Tags)
            .Include(t => t.TextFiles)
            .Include(t => t.Authors)
            .Include(t => t.Translators)
            .Include(t => t.Publisher)
            .Where(t => textsIds.Contains(t.Id))
            .GroupBy(t => t.Id)
            .ToDictionaryAsync(g => g.Key, g => g.First());
    }

    public async Task<TextDbo> GetTextMetadataByIdAsync(Guid textId)
    {
        return await _dbContext
            .Texts
            .Include(t => t.Tags)
            .Include(t => t.TextFiles)
            .Include(t => t.Authors)
            .Include(t => t.Translators)
            .Include(t => t.Publisher)
            .SingleAsync(t => t.Id == textId);
    }

    public async Task<int> GetTotalTextsCountAsync()
    {
        return await _dbContext
            .Texts
            .CountAsync();
    }

    public async Task<DateTime> GetLastTextAddTimeAsync()
    {
        return await _dbContext
            .Texts
            .MaxAsync(t => t.CreateTime);
    }

    public async Task<TextDbo> GetTextWithFilesByIdAsync(Guid textId)
    {
        return await _dbContext
            .Texts
            .Include(t => t.TextFiles)
            .ThenInclude(tf => tf.File)
            .SingleAsync(t => t.Id == textId);
    }

    public async Task<IReadOnlyCollection<TextFileDbo>> GetTextFilesByTextAsync(Guid textId)
    {
        return (await GetTextWithFilesByIdAsync(textId))
            .TextFiles
            .AsReadOnly();
    }

    public async Task<TextPageDbo> GetPageAsync(Guid textId, int pageNumber)
    {
        return (await _dbContext
            .Texts
            .Include(t => t.Pages)
            .ThenInclude(p => p.Sections)
            .ThenInclude(s => s.Variants)
            .SingleAsync(t => t.Id == textId))
            .Pages
            .Single(p => p.Number == pageNumber);
    }

    public async Task<IReadOnlyCollection<TextPageDbo>> GetAllPagesAsync(Guid textId)
    {
        return (await _dbContext
                .Texts
                .Include(t => t.Pages)
                .ThenInclude(p => p.Sections)
                .ThenInclude(s => s.Variants)
                .SingleAsync(t => t.Id == textId))
            .Pages
            .AsReadOnly();
    }

    public async Task<Dictionary<Guid, int>> GetPagesCountByTexts(IReadOnlyCollection<Guid> textsIds)
    {
        _ = textsIds ?? throw new ArgumentNullException(nameof(textsIds), "Texts IDs must not be null!");

        return await _dbContext
            .Texts
            .Include(t => t.Pages)
            .Where(t => textsIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, t => t.Pages.Count);
    }
}