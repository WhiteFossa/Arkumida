﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using webapi.Dao;

#nullable disable

namespace webapi.Migrations.SecurityDb
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20231104051908_AddPageToStatistics")]
    partial class AddPageToStatistics
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

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

            modelBuilder.Entity("TextsAuthors", b =>
                {
                    b.Property<Guid>("CreatureId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TextId")
                        .HasColumnType("uuid");

                    b.HasKey("CreatureId", "TextId");

                    b.HasIndex("TextId");

                    b.ToTable("TextsAuthors");
                });

            modelBuilder.Entity("TextsTranslators", b =>
                {
                    b.Property<Guid>("CreatureId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TextId")
                        .HasColumnType("uuid");

                    b.HasKey("CreatureId", "TextId");

                    b.HasIndex("TextId");

                    b.ToTable("TextsTranslators");
                });

            modelBuilder.Entity("webapi.Dao.Models.AvatarDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatureProfileId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("FileId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("UploadTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CreatureProfileId");

                    b.HasIndex("FileId");

                    b.ToTable("Avatars");
                });

            modelBuilder.Entity("webapi.Dao.Models.CreatureDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("webapi.Dao.Models.CreatureProfileDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("About")
                        .HasColumnType("text");

                    b.Property<Guid?>("CurrentAvatarId")
                        .HasColumnType("uuid");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text");

                    b.Property<bool>("IsPasswordChangeRequired")
                        .HasColumnType("boolean");

                    b.Property<string>("OneTimePlaintextPassword")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CurrentAvatarId");

                    b.ToTable("Profiles");
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

            modelBuilder.Entity("webapi.Dao.Models.PrivateMessageDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeletedOnReceiverSide")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeletedOnSenderSide")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ReadTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ReceiverId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SentTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("PrivateMessages");
                });

            modelBuilder.Entity("webapi.Dao.Models.RenderedTextDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("FileId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TextId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("TextId");

                    b.ToTable("RenderedTexts");
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

                    b.Property<Guid?>("PublisherId")
                        .HasColumnType("uuid");

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

                    b.HasIndex("PublisherId");

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

                    b.ToTable("TextPages");
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

            modelBuilder.Entity("webapi.Dao.Models.TextsStatisticsEventDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CausedByCreatureId")
                        .HasColumnType("uuid");

                    b.Property<string>("Ip")
                        .HasColumnType("text");

                    b.Property<int?>("Page")
                        .HasColumnType("integer");

                    b.Property<Guid?>("TextId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("UserAgent")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CausedByCreatureId");

                    b.HasIndex("TextId");

                    b.ToTable("TextsStatisticsEvents");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("webapi.Dao.Models.CreatureDbo", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("webapi.Dao.Models.CreatureDbo", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("webapi.Dao.Models.CreatureDbo", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("webapi.Dao.Models.CreatureDbo", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("TextsAuthors", b =>
                {
                    b.HasOne("webapi.Dao.Models.CreatureDbo", null)
                        .WithMany()
                        .HasForeignKey("CreatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("webapi.Dao.Models.TextDbo", null)
                        .WithMany()
                        .HasForeignKey("TextId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TextsTranslators", b =>
                {
                    b.HasOne("webapi.Dao.Models.CreatureDbo", null)
                        .WithMany()
                        .HasForeignKey("CreatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("webapi.Dao.Models.TextDbo", null)
                        .WithMany()
                        .HasForeignKey("TextId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("webapi.Dao.Models.AvatarDbo", b =>
                {
                    b.HasOne("webapi.Dao.Models.CreatureProfileDbo", "CreatureProfile")
                        .WithMany("Avatars")
                        .HasForeignKey("CreatureProfileId");

                    b.HasOne("webapi.Dao.Models.FileDbo", "File")
                        .WithMany()
                        .HasForeignKey("FileId");

                    b.Navigation("CreatureProfile");

                    b.Navigation("File");
                });

            modelBuilder.Entity("webapi.Dao.Models.CreatureProfileDbo", b =>
                {
                    b.HasOne("webapi.Dao.Models.AvatarDbo", "CurrentAvatar")
                        .WithMany()
                        .HasForeignKey("CurrentAvatarId");

                    b.Navigation("CurrentAvatar");
                });

            modelBuilder.Entity("webapi.Dao.Models.PrivateMessageDbo", b =>
                {
                    b.HasOne("webapi.Dao.Models.CreatureDbo", "Receiver")
                        .WithMany("ReceiverOfThisPrivateMessages")
                        .HasForeignKey("ReceiverId");

                    b.HasOne("webapi.Dao.Models.CreatureDbo", "Sender")
                        .WithMany("SenderOfThisPrivateMessages")
                        .HasForeignKey("SenderId");

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("webapi.Dao.Models.RenderedTextDbo", b =>
                {
                    b.HasOne("webapi.Dao.Models.FileDbo", "File")
                        .WithMany()
                        .HasForeignKey("FileId");

                    b.HasOne("webapi.Dao.Models.TextDbo", "Text")
                        .WithMany("RenderedTexts")
                        .HasForeignKey("TextId");

                    b.Navigation("File");

                    b.Navigation("Text");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextDbo", b =>
                {
                    b.HasOne("webapi.Dao.Models.CreatureDbo", "Publisher")
                        .WithMany()
                        .HasForeignKey("PublisherId");

                    b.Navigation("Publisher");
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

            modelBuilder.Entity("webapi.Dao.Models.TextsStatisticsEventDbo", b =>
                {
                    b.HasOne("webapi.Dao.Models.CreatureDbo", "CausedByCreature")
                        .WithMany()
                        .HasForeignKey("CausedByCreatureId");

                    b.HasOne("webapi.Dao.Models.TextDbo", "Text")
                        .WithMany()
                        .HasForeignKey("TextId");

                    b.Navigation("CausedByCreature");

                    b.Navigation("Text");
                });

            modelBuilder.Entity("webapi.Dao.Models.CreatureDbo", b =>
                {
                    b.Navigation("ReceiverOfThisPrivateMessages");

                    b.Navigation("SenderOfThisPrivateMessages");
                });

            modelBuilder.Entity("webapi.Dao.Models.CreatureProfileDbo", b =>
                {
                    b.Navigation("Avatars");
                });

            modelBuilder.Entity("webapi.Dao.Models.TextDbo", b =>
                {
                    b.Navigation("Pages");

                    b.Navigation("RenderedTexts");

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
