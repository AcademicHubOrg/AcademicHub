using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseStream.Data.Migrations
{
    /// <inheritdoc />
    public partial class TemplateIdAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TemplateId",
                table: "CourseStreams",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "CourseStreams");
        }
    }
}
