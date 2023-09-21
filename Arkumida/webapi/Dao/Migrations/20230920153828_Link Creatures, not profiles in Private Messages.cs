using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations.SecurityDb
{
    /// <inheritdoc />
    public partial class LinkCreaturesnotprofilesinPrivateMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessageDbo_Profiles_ReceiverId",
                table: "PrivateMessageDbo");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessageDbo_Profiles_SenderId",
                table: "PrivateMessageDbo");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessageDbo_AspNetUsers_ReceiverId",
                table: "PrivateMessageDbo",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessageDbo_AspNetUsers_SenderId",
                table: "PrivateMessageDbo",
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
