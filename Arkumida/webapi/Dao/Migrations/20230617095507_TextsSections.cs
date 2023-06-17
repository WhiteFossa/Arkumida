using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Dao.Migrations
{
    /// <inheritdoc />
    public partial class TextsSections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TextSectionDboId",
                table: "TextsSectionsVariants",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TextsSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalText = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextsSections", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TextsSectionsVariants_TextSectionDboId",
                table: "TextsSectionsVariants",
                column: "TextSectionDboId");

            migrationBuilder.AddForeignKey(
                name: "FK_TextsSectionsVariants_TextsSections_TextSectionDboId",
                table: "TextsSectionsVariants",
                column: "TextSectionDboId",
                principalTable: "TextsSections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
