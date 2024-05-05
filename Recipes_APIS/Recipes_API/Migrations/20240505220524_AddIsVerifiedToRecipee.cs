using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipe.Migrations
{
    /// <inheritdoc />
    public partial class AddIsVerifiedToRecipee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe6995fd-ed28-441f-b4cd-378ec0c046d9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1999cc8c-6623-486c-a637-12e027582365", "AQAAAAIAAYagAAAAEIDlLImZrG8MtJ2DZZ0WPSzbBHhchGNg+2ZeochE0uM3ZWFCz3vA+KV0JI2UzDIqdg==", "49b080ac-96fb-48d9-831f-a1578644a442" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Recipes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe6995fd-ed28-441f-b4cd-378ec0c046d9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "52df7cf1-3a25-436b-8737-022ec63421d3", "AQAAAAIAAYagAAAAELnVahEMh/uVzSW+uCLiAWuWJB8q60fhhdLxyBfZeDOOzxFVq7U27ZDmQxUPfq6WBQ==", "f0273abe-ed61-4d5a-bcfe-7250d4d6a447" });
        }
    }
}
