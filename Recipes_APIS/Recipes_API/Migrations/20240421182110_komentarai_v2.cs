using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipe.Migrations
{
    /// <inheritdoc />
    public partial class komentarai_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe6995fd-ed28-441f-b4cd-378ec0c046d9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "52df7cf1-3a25-436b-8737-022ec63421d3", "AQAAAAIAAYagAAAAELnVahEMh/uVzSW+uCLiAWuWJB8q60fhhdLxyBfZeDOOzxFVq7U27ZDmQxUPfq6WBQ==", "f0273abe-ed61-4d5a-bcfe-7250d4d6a447" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Comments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe6995fd-ed28-441f-b4cd-378ec0c046d9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cf49da3a-dbd1-47dd-9544-f46c4980ab0e", "AQAAAAIAAYagAAAAELOgIxwUNokSqkQewZ3yHmEEmCy44szVNG8NgrB4ynsL+VbGEmVBTshqqKWRvT/eSw==", "f1eda083-d279-4ddb-8524-85cb5a1fcd9f" });
        }
    }
}
