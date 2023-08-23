using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations.SecurityDb
{
    /// <inheritdoc />
    public partial class CreaturesRefactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avatars_AspNetUsers_CreatureId",
                table: "Avatars");

            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "Avatars");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OneTimePlaintextPassword",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CreatureId",
                table: "Avatars",
                newName: "CreatureProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Avatars_CreatureId",
                table: "Avatars",
                newName: "IX_Avatars_CreatureProfileId");

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OneTimePlaintextPassword = table.Column<string>(type: "text", nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    CurrentAvatarId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_Avatars_CurrentAvatarId",
                        column: x => x.CurrentAvatarId,
                        principalTable: "Avatars",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_CurrentAvatarId",
                table: "Profiles",
                column: "CurrentAvatarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_Profiles_CreatureProfileId",
                table: "Avatars",
                column: "CreatureProfileId",
                principalTable: "Profiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
