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
        
        // Loading tags
        if (text.Tags != null)
        {
            text.Tags = text
                .Tags
                .Select(tag => _dbContext.Tags.Single(t => t.Id == tag.Id))
                .ToList();
        }
        
        await _dbContext
            .Texts
            .AddAsync(text);
        
        await _dbContext.SaveChangesAsync();
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

    public async Task<IReadOnlyCollection<TextDbo>> GetTextsMetadataAsync(TextOrderMode orderMode, int skip, int take)
    {
        if (skip < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skip), "Skip must not be negative.");
        }

        if (take <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(take), "Take must be positive.");
        }

        IQueryable<TextDbo> orderedSource = _dbContext
            .Texts
            .Include(t => t.Tags)
            .Include(t => t.TextFiles);
        
        switch (orderMode)
        {
            case TextOrderMode.Latest:
                orderedSource = orderedSource.OrderByDescending(t => t.CreateTime);
                break;
            
            case TextOrderMode.Popular:
                orderedSource = orderedSource.OrderByDescending(t => t.ReadsCount);
                break;
            
            default:
                throw new ArgumentException("Unknown ordering mode.", nameof(orderMode));
        }

        return await orderedSource
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<TextDbo> GetTextMetadataByIdAsync(Guid textId)
    {
        return await _dbContext
            .Texts
            .Include(t => t.Tags)
            .Include(t => t.TextFiles)
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

    public async Task<int> GetPagesCountByTextId(Guid textId)
    {
        return (await _dbContext
            .Texts
            .Include(t => t.Pages)
            .SingleAsync(t => t.Id == textId))
            .Pages
            .Count;
    }
}