using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations.SecurityDb
{
    /// <inheritdoc />
    public partial class AddedUsersToText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Texts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublisherId",
                table: "Texts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranslatorId",
                table: "Texts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Texts_AuthorId",
                table: "Texts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Texts_PublisherId",
                table: "Texts",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Texts_TranslatorId",
                table: "Texts",
                column: "TranslatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Texts_AspNetUsers_AuthorId",
                table: "Texts",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Texts_AspNetUsers_PublisherId",
                table: "Texts",
                column: "PublisherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Texts_AspNetUsers_TranslatorId",
                table: "Texts",
                column: "TranslatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
