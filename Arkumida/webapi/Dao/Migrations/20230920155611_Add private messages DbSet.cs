using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations.SecurityDb
{
    /// <inheritdoc />
    public partial class AddprivatemessagesDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessageDbo_AspNetUsers_ReceiverId",
                table: "PrivateMessageDbo");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessageDbo_AspNetUsers_SenderId",
                table: "PrivateMessageDbo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrivateMessageDbo",
                table: "PrivateMessageDbo");

            migrationBuilder.RenameTable(
                name: "PrivateMessageDbo",
                newName: "PrivateMessages");

            migrationBuilder.RenameIndex(
                name: "IX_PrivateMessageDbo_SenderId",
                table: "PrivateMessages",
                newName: "IX_PrivateMessages_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_PrivateMessageDbo_ReceiverId",
                table: "PrivateMessages",
                newName: "IX_PrivateMessages_ReceiverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrivateMessages",
                table: "PrivateMessages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessages_AspNetUsers_ReceiverId",
                table: "PrivateMessages",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessages_AspNetUsers_SenderId",
                table: "PrivateMessages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
