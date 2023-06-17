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

    /// <summary>
    /// Texts sections variants
    /// </summary>
    public DbSet<TextSectionVariantDbo> TextsSectionsVariants { get; set; }

    /// <summary>
    /// Texts sections
    /// </summary>
    public DbSet<TextSectionDbo> TextsSections { get; set; }

    /// <summary>
    /// Texts
    /// </summary>
    public DbSet<TextDbo> Texts { get; set; }


    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Variants are parts of sections
        modelBuilder
            .Entity<TextSectionDbo>()
            .HasMany(ts => ts.Variants);
        
        // Sections are parts of texts
        modelBuilder
            .Entity<TextDbo>()
            .HasMany(t => t.Sections);
    }
}