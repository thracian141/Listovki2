using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Listovki.Data.Migrations
{
    /// <inheritdoc />
    public partial class _3305A2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ListovkaResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "ListovkaResults");
        }
    }
}
