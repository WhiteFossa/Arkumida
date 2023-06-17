using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Dao.Migrations
{
    /// <inheritdoc />
    public partial class Texts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TextDboId",
                table: "TextsSections",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Texts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Texts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TextsSections_TextDboId",
                table: "TextsSections",
                column: "TextDboId");

            migrationBuilder.AddForeignKey(
                name: "FK_TextsSections_Texts_TextDboId",
                table: "TextsSections",
                column: "TextDboId",
                principalTable: "Texts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
