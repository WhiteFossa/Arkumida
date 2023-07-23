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

    /// <summary>
    /// Files
    /// </summary>
    public DbSet<FileDbo> Files { get; set; }

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Variants are parts of sections
        modelBuilder
            .Entity<TextSectionDbo>()
            .HasMany(ts => ts.Variants);
        
        // Text have many pages
        modelBuilder
            .Entity<TextDbo>()
            .HasMany(t => t.Pages);
        
        // Page have many sections
        modelBuilder
            .Entity<TextPageDbo>()
            .HasMany(p => p.Sections);

        // Text have many tags, tag have many texts
        modelBuilder
            .Entity<TextDbo>()
            .HasMany(text => text.Tags)
            .WithMany(tag => tag.Texts);
        
        // Text have many files
        modelBuilder
            .Entity<TextDbo>()
            .HasMany(t => t.TextFiles);
    }
}