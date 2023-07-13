using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Dao.Migrations
{
    /// <inheritdoc />
    public partial class AddFilesToText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TextFileDbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    FileId = table.Column<Guid>(type: "uuid", nullable: true),
                    TextDboId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextFileDbo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextFileDbo_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TextFileDbo_Texts_TextDboId",
                        column: x => x.TextDboId,
                        principalTable: "Texts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TextFileDbo_FileId",
                table: "TextFileDbo",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_TextFileDbo_TextDboId",
                table: "TextFileDbo",
                column: "TextDboId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
