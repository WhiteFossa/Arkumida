#region License
// Arkumida - Furtails.pw next generation backend
// Copyright (C) 2023  Earlybeasts
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webapi.Dao.Models;
using webapi.Dao.Models.Enums.Statistics;
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

    /// <summary>
    /// Private messages
    /// </summary>
    public DbSet<PrivateMessageDbo> PrivateMessages { get; set; }

    /// <summary>
    /// Texts statistics events
    /// </summary>
    public DbSet<TextsStatisticsEventDbo> TextsStatisticsEvents { get; set; }

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
        
        // Private message have one sender
        modelBuilder
            .Entity<PrivateMessageDbo>()
            .HasOne(pm => pm.Sender)
            .WithMany(cp => cp.SenderOfThisPrivateMessages);
        
        // Private message have one receiver
        modelBuilder
            .Entity<PrivateMessageDbo>()
            .HasOne(pm => pm.Receiver)
            .WithMany(cp => cp.ReceiverOfThisPrivateMessages);
        
        // Statistics event have one related text
        modelBuilder
            .Entity<TextsStatisticsEventDbo>()
            .HasOne(tse => tse.Text);
        
        // Statistics event may have one related creature
        modelBuilder
            .Entity<TextsStatisticsEventDbo>()
            .HasOne(tse => tse.CausedByCreature);
    }
}