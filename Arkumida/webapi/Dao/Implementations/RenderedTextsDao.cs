using Microsoft.EntityFrameworkCore;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Dao.Models.Enums.RenderedTexts;

namespace webapi.Dao.Implementations;

public class RenderedTextsDao : IRenderedTextsDao
{
    private readonly MainDbContext _dbContext;

    public RenderedTextsDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateRenderedTextAsync(RenderedTextDbo renderedText)
    {
        _ = renderedText ?? throw new ArgumentNullException(nameof(renderedText), "Rendered text must not be null");

        // Loading text
        renderedText.Text = _dbContext.Texts.Single(t => t.Id == renderedText.Text.Id);
        
        // Loading file
        renderedText.File = _dbContext.Files.Single(f => f.Id == renderedText.File.Id);
        
        await _dbContext
            .RenderedTexts
            .AddAsync(renderedText);
        
        await _dbContext
            .SaveChangesAsync();
    }

    public async Task<RenderedTextDbo> GetRenderedTextAsync(Guid textId, RenderedTextType type)
    {
        return await _dbContext
            .RenderedTexts
            .Include(rt => rt.Text)
            .Include(rt => rt.File)
            .SingleOrDefaultAsync(rt => rt.Id == textId && rt.Type == type);
    }
}