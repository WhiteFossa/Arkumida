using Microsoft.EntityFrameworkCore;
using webapi.Dao.Abstract;
using webapi.Dao.Models;

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
        
        await _dbContext
            .Texts
            .AddAsync(text);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddSectionToText(Guid textId, Guid sectionId)
    {
        var text = _dbContext
            .Texts
            .Include(t => t.Sections)
            .Single(t => t.Id == textId);

        var section = _dbContext
            .TextsSections
            .Single(ts => ts.Id == sectionId);
        
        text.Sections.Add(section);

        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to update 1 row, actually updated { affected } rows!");
        }
    }
}