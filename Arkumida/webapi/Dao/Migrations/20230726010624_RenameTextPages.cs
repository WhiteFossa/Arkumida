using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Dao.Migrations
{
    /// <inheritdoc />
    public partial class RenameTextPages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextPageDbo_Texts_TextDboId",
                table: "TextPageDbo");

            migrationBuilder.DropForeignKey(
                name: "FK_TextsSections_TextPageDbo_TextPageDboId",
                table: "TextsSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TextPageDbo",
                table: "TextPageDbo");

            migrationBuilder.RenameTable(
                name: "TextPageDbo",
                newName: "TextPages");

            migrationBuilder.RenameIndex(
                name: "IX_TextPageDbo_TextDboId",
                table: "TextPages",
                newName: "IX_TextPages_TextDboId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextPages",
                table: "TextPages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TextPages_Texts_TextDboId",
                table: "TextPages",
                column: "TextDboId",
                principalTable: "Texts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TextsSections_TextPages_TextPageDboId",
                table: "TextsSections",
                column: "TextPageDboId",
                principalTable: "TextPages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
