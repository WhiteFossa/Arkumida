﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using webapi.Dao;

#nullable disable

namespace webapi.Dao.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("webapi.Dao.Models.TagDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("CategoryOrder")
                        .HasColumnType("integer");

                    b.Property<int>("CategoryType")
                        .HasColumnType("integer");

                    b.Property<string>("FurryReadableId")
                        .HasColumnType("text");

                    b.Property<bool>("IsCategory")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Subtype")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Texts");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextSectionDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("OriginalText")
                        .HasColumnType("text");

                    b.Property<Guid?>("TextDboId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TextDboId");

                    b.ToTable("TextsSections");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextSectionVariantDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("TextSectionDboId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TextSectionDboId");

                    b.ToTable("TextsSectionsVariants");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextSectionDbo", b =>
                {
                    b.HasOne("webapi.Dao.Models.TextDbo", null)
                        .WithMany("Sections")
                        .HasForeignKey("TextDboId");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextSectionVariantDbo", b =>
                {
                    b.HasOne("webapi.Dao.Models.TextSectionDbo", null)
                        .WithMany("Variants")
                        .HasForeignKey("TextSectionDboId");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextDbo", b =>
                {
                    b.Navigation("Sections");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextSectionDbo", b =>
                {
                    b.Navigation("Variants");
                });
#pragma warning restore 612, 618
        }
    }
}