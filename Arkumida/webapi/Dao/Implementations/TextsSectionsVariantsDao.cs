using webapi.Dao.Abstract;
using webapi.Dao.Models;

namespace webapi.Dao.Implementations;

public class TextsSectionsVariantsDao : ITextsSectionsVariantsDao
{
    private readonly MainDbContext _dbContext;
    
    public TextsSectionsVariantsDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateTextSectionVariantAsync(TextSectionVariantDbo variant)
    {
        _ = variant ?? throw new ArgumentNullException(nameof(variant), "Variant must not be null.");
        
        await _dbContext
            .TextsSectionsVariants
            .AddAsync(variant);
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to insert 1 row, actually inserted { affected } rows!");
        }
    }
}