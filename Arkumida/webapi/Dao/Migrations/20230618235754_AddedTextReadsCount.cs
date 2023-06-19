using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Dao.Migrations
{
    /// <inheritdoc />
    public partial class AddedTextReadsCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ReadsCount",
                table: "Texts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
