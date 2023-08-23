using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webapi.Dao.Models;
using webapi.Models;

namespace webapi.Dao;

/// <summary>
/// Main DB context (security information live here too)
/// </summary>
public class MainDbContext : IdentityDbContext<CreatureDbo, IdentityRole<Guid>, Guid>
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
    /// Text pages
    /// </summary>
    public DbSet<TextPageDbo> TextPages { get; set; }

    /// <summary>
    /// Texts
    /// </summary>
    public DbSet<TextDbo> Texts { get; set; }

    /// <summary>
    /// Files
    /// </summary>
    public DbSet<FileDbo> Files { get; set; }

    /// <summary>
    /// Rendered texts
    /// </summary>
    public DbSet<RenderedTextDbo> RenderedTexts { get; set; }

    /// <summary>
    /// Avatars
    /// </summary>
    public DbSet<AvatarDbo> Avatars { get; set; }

    /// <summary>
    /// Creature profiles
    /// </summary>
    public DbSet<CreatureProfileDbo> Profiles { get; set; }

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
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
        
        // Text have many authors
        modelBuilder
            .Entity<TextDbo>()
            .HasMany(text => text.Authors)
            .WithMany(creature => creature.TextsAuthor)
            .UsingEntity<Dictionary<string, object>>
            (
                "TextsAuthors",
                jt => jt.HasOne<CreatureDbo>().WithMany().HasForeignKey("CreatureId"),
                jt => jt.HasOne<TextDbo>().WithMany().HasForeignKey("TextId")
            );
        
        // Text have many translators
        modelBuilder
            .Entity<TextDbo>()
            .HasMany(text => text.Translators)
            .WithMany(creature => creature.TextsTranslator)
            .UsingEntity<Dictionary<string, object>>
            (
                "TextsTranslators",
                jt => jt.HasOne<CreatureDbo>().WithMany().HasForeignKey("CreatureId"),
                jt => jt.HasOne<TextDbo>().WithMany().HasForeignKey("TextId")
            );
        
        // Text have one publisher
        modelBuilder
            .Entity<TextDbo>()
            .HasOne(text => text.Publisher);
        
        // Text have many rendered texts
        modelBuilder
            .Entity<TextDbo>()
            .HasMany(t => t.RenderedTexts)
            .WithOne(rt => rt.Text);
        
        // Profile have many avatars
        modelBuilder
            .Entity<CreatureProfileDbo>()
            .HasMany(p => p.Avatars)
            .WithOne(a => a.CreatureProfile);
    }
}