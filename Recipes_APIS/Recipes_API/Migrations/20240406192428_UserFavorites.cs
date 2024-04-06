using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipe.Migrations
{
    /// <inheritdoc />
    public partial class UserFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFavoriteRecipes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecipeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteRecipes", x => new { x.UserId, x.RecipeeId });
                    table.ForeignKey(
                        name: "FK_UserFavoriteRecipes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavoriteRecipes_Recipes_RecipeeId",
                        column: x => x.RecipeeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe6995fd-ed28-441f-b4cd-378ec0c046d9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e963db64-f0f8-4704-a3e2-57771fc546bb", "AQAAAAIAAYagAAAAEJouLILZTqRKfFjrezbWWFY/AhY0vgWsQE1rPovBpKMnPgsR0Pbvr/OpU0SzYR0Wdw==", "5be86b93-b66f-41b2-80b3-acdb93b89b67" });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteRecipes_RecipeeId",
                table: "UserFavoriteRecipes",
                column: "RecipeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoriteRecipes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fe6995fd-ed28-441f-b4cd-378ec0c046d9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4fcc91be-b4bf-4173-9f05-7d89f514e208", "AQAAAAIAAYagAAAAENh0jK97l/5HvuCTuqpztwhcD+R3RaUeLufZ8pGBH/KHFf9r1W4DquJRPxoLQcJuvg==", "56f942e4-1a67-4a99-8504-9cc3ada2251e" });
        }
    }
}
