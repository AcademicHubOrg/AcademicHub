using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Materials.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTextData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataText",
                table: "Materials",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataText",
                table: "Materials");
        }
    }
}
