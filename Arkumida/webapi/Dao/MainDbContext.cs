using Microsoft.EntityFrameworkCore;

namespace webapi.Dao;

/// <summary>
/// Main DB context
/// </summary>
public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}