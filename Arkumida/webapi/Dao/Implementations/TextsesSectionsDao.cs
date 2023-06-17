using Microsoft.EntityFrameworkCore;
using webapi.Dao.Abstract;
using webapi.Dao.Models;

namespace webapi.Dao.Implementations;

public class TextsesSectionsDao : ITextsSectionsDao
{
    private readonly MainDbContext _dbContext;
    
    public TextsesSectionsDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateTextSectionAsync(TextSectionDbo section)
    {
        _ = section ?? throw new ArgumentNullException(nameof(section), "Section must not be null.");

        if (section.Variants.Any())
        {
            throw new InvalidOperationException("Do not pass variants when creating a section, attach them later.");
        }
        
        await _dbContext
            .TextsSections
            .AddAsync(section);
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to insert 1 row, actually inserted { affected } rows!");
        }
    }

    public async Task AddVariantToSection(Guid sectionId, Guid variantId)
    {
        var section = _dbContext
            .TextsSections
            .Include(s => s.Variants)
            .Single(s => s.Id == sectionId);

        var variant = _dbContext
            .TextsSectionsVariants
            .Single(v => v.Id == variantId);
        
        section.Variants.Add(variant);
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to update 1 row, actually updated { affected } rows!");
        }
    }
}