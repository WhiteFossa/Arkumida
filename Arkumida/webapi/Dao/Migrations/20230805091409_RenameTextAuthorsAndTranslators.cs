using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations.SecurityDb
{
    /// <inheritdoc />
    public partial class RenameTextAuthorsAndTranslators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextsAuthors_AspNetUsers_AuthorsId",
                table: "TextsAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_TextsAuthors_Texts_TextsAuthorId",
                table: "TextsAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_TextsTranslators_AspNetUsers_TranslatorsId",
                table: "TextsTranslators");

            migrationBuilder.DropForeignKey(
                name: "FK_TextsTranslators_Texts_TextsTranslatorId",
                table: "TextsTranslators");

            migrationBuilder.RenameColumn(
                name: "TranslatorsId",
                table: "TextsTranslators",
                newName: "TextId");

            migrationBuilder.RenameColumn(
                name: "TextsTranslatorId",
                table: "TextsTranslators",
                newName: "CreatureId");

            migrationBuilder.RenameIndex(
                name: "IX_TextsTranslators_TranslatorsId",
                table: "TextsTranslators",
                newName: "IX_TextsTranslators_TextId");

            migrationBuilder.RenameColumn(
                name: "TextsAuthorId",
                table: "TextsAuthors",
                newName: "TextId");

            migrationBuilder.RenameColumn(
                name: "AuthorsId",
                table: "TextsAuthors",
                newName: "CreatureId");

            migrationBuilder.RenameIndex(
                name: "IX_TextsAuthors_TextsAuthorId",
                table: "TextsAuthors",
                newName: "IX_TextsAuthors_TextId");

            migrationBuilder.AddForeignKey(
                name: "FK_TextsAuthors_AspNetUsers_CreatureId",
                table: "TextsAuthors",
                column: "CreatureId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TextsAuthors_Texts_TextId",
                table: "TextsAuthors",
                column: "TextId",
                principalTable: "Texts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TextsTranslators_AspNetUsers_CreatureId",
                table: "TextsTranslators",
                column: "CreatureId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TextsTranslators_Texts_TextId",
                table: "TextsTranslators",
                column: "TextId",
                principalTable: "Texts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
