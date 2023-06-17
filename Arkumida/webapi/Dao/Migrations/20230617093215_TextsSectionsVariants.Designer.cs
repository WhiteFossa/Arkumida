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
    [Migration("20230617093215_TextsSectionsVariants")]
    partial class TextsSectionsVariants
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("webapi.Dao.Models.TextSectionVariantDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("TextsSectionsVariants");
                });
#pragma warning restore 612, 618
        }
    }
}