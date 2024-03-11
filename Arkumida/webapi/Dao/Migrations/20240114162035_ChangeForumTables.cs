using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations.SecurityDb
{
    /// <inheritdoc />
    public partial class ChangeForumTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessages_ForumTopics_ForumTopicId",
                table: "ForumMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumSections_ForumSections_ForumSectionId",
                table: "ForumSections");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumTopics_ForumSections_ForumSectionId",
                table: "ForumTopics");

            migrationBuilder.RenameColumn(
                name: "ForumSectionId",
                table: "ForumTopics",
                newName: "ForumSectionDboId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumTopics_ForumSectionId",
                table: "ForumTopics",
                newName: "IX_ForumTopics_ForumSectionDboId");

            migrationBuilder.RenameColumn(
                name: "ForumSectionId",
                table: "ForumSections",
                newName: "ForumSectionDboId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumSections_ForumSectionId",
                table: "ForumSections",
                newName: "IX_ForumSections_ForumSectionDboId");

            migrationBuilder.RenameColumn(
                name: "ForumTopicId",
                table: "ForumMessages",
                newName: "ForumTopicDboId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumMessages_ForumTopicId",
                table: "ForumMessages",
                newName: "IX_ForumMessages_ForumTopicDboId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessages_ForumTopics_ForumTopicDboId",
                table: "ForumMessages",
                column: "ForumTopicDboId",
                principalTable: "ForumTopics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumSections_ForumSections_ForumSectionDboId",
                table: "ForumSections",
                column: "ForumSectionDboId",
                principalTable: "ForumSections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumTopics_ForumSections_ForumSectionDboId",
                table: "ForumTopics",
                column: "ForumSectionDboId",
                principalTable: "ForumSections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessages_ForumTopics_ForumTopicDboId",
                table: "ForumMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumSections_ForumSections_ForumSectionDboId",
                table: "ForumSections");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumTopics_ForumSections_ForumSectionDboId",
                table: "ForumTopics");

            migrationBuilder.RenameColumn(
                name: "ForumSectionDboId",
                table: "ForumTopics",
                newName: "ForumSectionId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumTopics_ForumSectionDboId",
                table: "ForumTopics",
                newName: "IX_ForumTopics_ForumSectionId");

            migrationBuilder.RenameColumn(
                name: "ForumSectionDboId",
                table: "ForumSections",
                newName: "ForumSectionId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumSections_ForumSectionDboId",
                table: "ForumSections",
                newName: "IX_ForumSections_ForumSectionId");

            migrationBuilder.RenameColumn(
                name: "ForumTopicDboId",
                table: "ForumMessages",
                newName: "ForumTopicId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumMessages_ForumTopicDboId",
                table: "ForumMessages",
                newName: "IX_ForumMessages_ForumTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessages_ForumTopics_ForumTopicId",
                table: "ForumMessages",
                column: "ForumTopicId",
                principalTable: "ForumTopics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumSections_ForumSections_ForumSectionId",
                table: "ForumSections",
                column: "ForumSectionId",
                principalTable: "ForumSections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumTopics_ForumSections_ForumSectionId",
                table: "ForumTopics",
                column: "ForumSectionId",
                principalTable: "ForumSections",
                principalColumn: "Id");
        }
    }
}
