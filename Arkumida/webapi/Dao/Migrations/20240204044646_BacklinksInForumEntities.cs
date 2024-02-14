using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations.SecurityDb
{
    /// <inheritdoc />
    public partial class BacklinksInForumEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessages_ForumTopics_ForumTopicDboId",
                table: "ForumMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumTopics_ForumSections_ForumSectionDboId",
                table: "ForumTopics");

            migrationBuilder.DropIndex(
                name: "IX_ForumTopics_ForumSectionDboId",
                table: "ForumTopics");

            migrationBuilder.DropIndex(
                name: "IX_ForumMessages_ForumTopicDboId",
                table: "ForumMessages");

            migrationBuilder.DropColumn(
                name: "ForumSectionDboId",
                table: "ForumTopics");

            migrationBuilder.DropColumn(
                name: "ForumTopicDboId",
                table: "ForumMessages");

            migrationBuilder.AddColumn<Guid>(
                name: "ForumSectionId",
                table: "ForumTopics",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ForumTopicId",
                table: "ForumMessages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopics_ForumSectionId",
                table: "ForumTopics",
                column: "ForumSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumMessages_ForumTopicId",
                table: "ForumMessages",
                column: "ForumTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessages_ForumTopics_ForumTopicId",
                table: "ForumMessages",
                column: "ForumTopicId",
                principalTable: "ForumTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumTopics_ForumSections_ForumSectionId",
                table: "ForumTopics",
                column: "ForumSectionId",
                principalTable: "ForumSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
