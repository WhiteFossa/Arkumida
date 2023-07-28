﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using webapi.Dao;

#nullable disable

namespace webapi.Dao.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20230723005836_AddPages")]
    partial class AddPages
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TagDboTextDbo", b =>
                {
                    b.Property<Guid>("TagsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TextsId")
                        .HasColumnType("uuid");

                    b.HasKey("TagsId", "TextsId");

                    b.HasIndex("TextsId");

                    b.ToTable("TagDboTextDbo");
                });

            modelBuilder.Entity("webapi.Dao.Models.FileDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Content")
                        .HasColumnType("bytea");

                    b.Property<string>("Hash")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModifiedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

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

                    b.Property<bool>("IsHidden")
                        .HasColumnType("boolean");

                    b.Property<int>("Meaning")
                        .HasColumnType("integer");

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

                    b.Property<bool>("IsIncomplete")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("ReadsCount")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<long>("VotesCount")
                        .HasColumnType("bigint");

                    b.Property<long>("VotesMinus")
                        .HasColumnType("bigint");

                    b.Property<long>("VotesPlus")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Texts");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextFileDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("FileId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("TextDboId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("TextDboId");

                    b.ToTable("TextFileDbo");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextPageDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<Guid?>("TextDboId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TextDboId");

                    b.ToTable("TextPageDbo");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextSectionDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<string>("OriginalText")
                        .HasColumnType("text");

                    b.Property<Guid?>("TextPageDboId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TextPageDboId");

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

            modelBuilder.Entity("TagDboTextDbo", b =>
                {
                    b.HasOne("webapi.Dao.Models.TagDbo", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("webapi.Dao.Models.TextDbo", null)
                        .WithMany()
                        .HasForeignKey("TextsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("webapi.Dao.Models.TextFileDbo", b =>
                {
                    b.HasOne("webapi.Dao.Models.FileDbo", "File")
                        .WithMany()
                        .HasForeignKey("FileId");

                    b.HasOne("webapi.Dao.Models.TextDbo", null)
                        .WithMany("TextFiles")
                        .HasForeignKey("TextDboId");

                    b.Navigation("File");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextPageDbo", b =>
                {
                    b.HasOne("webapi.Dao.Models.TextDbo", null)
                        .WithMany("Pages")
                        .HasForeignKey("TextDboId");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextSectionDbo", b =>
                {
                    b.HasOne("webapi.Dao.Models.TextPageDbo", null)
                        .WithMany("Sections")
                        .HasForeignKey("TextPageDboId");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextSectionVariantDbo", b =>
                {
                    b.HasOne("webapi.Dao.Models.TextSectionDbo", null)
                        .WithMany("Variants")
                        .HasForeignKey("TextSectionDboId");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextDbo", b =>
                {
                    b.Navigation("Pages");

                    b.Navigation("TextFiles");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextPageDbo", b =>
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
