using Microsoft.EntityFrameworkCore;
using webapi.Dao.Models;

namespace webapi.Dao;

/// <summary>
/// Main DB context
/// </summary>
public class MainDbContext : DbContext
{
    /// <summary>
    /// Tags
    /// </summary>
    public DbSet<TagDbo> Tags { get; set; }
    
    
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}