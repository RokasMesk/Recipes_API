using System;
using Microsoft.EntityFrameworkCore.Migrations;
#pragma warning disable CS8981
#nullable disable

namespace Recipe.Migrations
{
    /// <inheritdoc />
    public partial class komentarai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe6995fd-ed28-441f-b4cd-378ec0c046d9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cf49da3a-dbd1-47dd-9544-f46c4980ab0e", "AQAAAAIAAYagAAAAELOgIxwUNokSqkQewZ3yHmEEmCy44szVNG8NgrB4ynsL+VbGEmVBTshqqKWRvT/eSw==", "f1eda083-d279-4ddb-8524-85cb5a1fcd9f" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RecipeId",
                table: "Comments",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe6995fd-ed28-441f-b4cd-378ec0c046d9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fa4af37d-002c-4a95-9b8b-769109e16f88", "AQAAAAIAAYagAAAAEOIJgJCW3ozFyFfxFkOhMnPHG6CJsKPV99k3LH7ZmVAH/0QlmaEAknos1SDPp2Wfjg==", "c4def582-5698-4744-b366-a88c2e1e3f73" });
        }
    }
}
