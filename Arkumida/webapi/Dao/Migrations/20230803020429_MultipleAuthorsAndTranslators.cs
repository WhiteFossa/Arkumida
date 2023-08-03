using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations.SecurityDb
{
    /// <inheritdoc />
    public partial class MultipleAuthorsAndTranslators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Texts_AspNetUsers_AuthorId",
                table: "Texts");

            migrationBuilder.DropForeignKey(
                name: "FK_Texts_AspNetUsers_TranslatorId",
                table: "Texts");

            migrationBuilder.DropIndex(
                name: "IX_Texts_AuthorId",
                table: "Texts");

            migrationBuilder.DropIndex(
                name: "IX_Texts_TranslatorId",
                table: "Texts");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Texts");

            migrationBuilder.DropColumn(
                name: "TranslatorId",
                table: "Texts");

            migrationBuilder.CreateTable(
                name: "TextsAuthors",
                columns: table => new
                {
                    AuthorsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TextsAuthorId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextsAuthors", x => new { x.AuthorsId, x.TextsAuthorId });
                    table.ForeignKey(
                        name: "FK_TextsAuthors_AspNetUsers_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TextsAuthors_Texts_TextsAuthorId",
                        column: x => x.TextsAuthorId,
                        principalTable: "Texts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextsTranslators",
                columns: table => new
                {
                    TextsTranslatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    TranslatorsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextsTranslators", x => new { x.TextsTranslatorId, x.TranslatorsId });
                    table.ForeignKey(
                        name: "FK_TextsTranslators_AspNetUsers_TranslatorsId",
                        column: x => x.TranslatorsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TextsTranslators_Texts_TextsTranslatorId",
                        column: x => x.TextsTranslatorId,
                        principalTable: "Texts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TextsAuthors_TextsAuthorId",
                table: "TextsAuthors",
                column: "TextsAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_TextsTranslators_TranslatorsId",
                table: "TextsTranslators",
                column: "TranslatorsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
