using Microsoft.EntityFrameworkCore;
using webapi.Dao.Abstract;
using webapi.Dao.Models;

namespace webapi.Dao.Implementations;

public class FilesDao : IFilesDao
{
    private readonly MainDbContext _dbContext;

    public FilesDao
    (
        MainDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateFileAsync(FileDbo file)
    {
        _ = file ?? throw new ArgumentNullException(nameof(file), "File must not be null.");
        
        file.LastModifiedTime = DateTime.UtcNow;

        await _dbContext
            .Files
            .AddAsync(file);
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to insert 1 row, actually inserted { affected } rows!");
        }
    }

    public async Task DeleteFileAsync(Guid fileId)
    {
        var file = await _dbContext
            .Files
            .SingleAsync(f => f.Id == fileId);

        _dbContext.Remove(file);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<FileDbo> GetFileAsync(Guid fileId)
    {
        return await _dbContext
            .Files
            .SingleOrDefaultAsync(f => f.Id == fileId);
    }
}