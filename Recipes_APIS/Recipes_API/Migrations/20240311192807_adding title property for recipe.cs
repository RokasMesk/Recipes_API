using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipe.Migrations
{
    /// <inheritdoc />
    public partial class addingtitlepropertyforrecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Recipes");
        }
    }
}
