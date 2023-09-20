using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations.SecurityDb
{
    /// <inheritdoc />
    public partial class Addprivatemessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrivateMessageDbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReceiverId = table.Column<Guid>(type: "uuid", nullable: true),
                    SentTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReadTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateMessageDbo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateMessageDbo_Profiles_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrivateMessageDbo_Profiles_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessageDbo_ReceiverId",
                table: "PrivateMessageDbo",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessageDbo_SenderId",
                table: "PrivateMessageDbo",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
