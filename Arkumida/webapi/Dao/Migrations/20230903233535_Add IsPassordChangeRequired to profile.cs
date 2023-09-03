using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations.SecurityDb
{
    /// <inheritdoc />
    public partial class AddIsPassordChangeRequiredtoprofile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPasswordChangeRequired",
                table: "Profiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
