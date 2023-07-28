using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Dao.Migrations
{
    /// <inheritdoc />
    public partial class AddPages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextsSections_Texts_TextDboId",
                table: "TextsSections");

            migrationBuilder.RenameColumn(
                name: "TextDboId",
                table: "TextsSections",
                newName: "TextPageDboId");

            migrationBuilder.RenameIndex(
                name: "IX_TextsSections_TextDboId",
                table: "TextsSections",
                newName: "IX_TextsSections_TextPageDboId");

            migrationBuilder.CreateTable(
                name: "TextPageDbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    TextDboId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextPageDbo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextPageDbo_Texts_TextDboId",
                        column: x => x.TextDboId,
                        principalTable: "Texts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TextPageDbo_TextDboId",
                table: "TextPageDbo",
                column: "TextDboId");

            migrationBuilder.AddForeignKey(
                name: "FK_TextsSections_TextPageDbo_TextPageDboId",
                table: "TextsSections",
                column: "TextPageDboId",
                principalTable: "TextPageDbo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
