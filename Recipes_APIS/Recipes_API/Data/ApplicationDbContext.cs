using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recipe.Models;

namespace Recipe.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Recipee> Recipes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<RecipeType> RecipeTypes { get; set; }
        public DbSet<UserFavoriteRecipe> UserFavoriteRecipes { get; set; }
        public DbSet<UserRecipeRating> UserRecipeRatings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserFavoriteRecipe>()
                .HasKey(ufr => new { ufr.UserId, ufr.RecipeeId });

            builder.Entity<UserFavoriteRecipe>()
                .HasOne(ufr => ufr.User)
                .WithMany(u => u.FavoriteRecipes)
                .HasForeignKey(ufr => ufr.UserId);

            builder.Entity<UserFavoriteRecipe>()
                .HasOne(ufr => ufr.Recipee)
                .WithMany(r => r.UserFavorites)
                .HasForeignKey(ufr => ufr.RecipeeId);

            builder.Entity<UserRecipeRating>()
               .HasKey(ur => new { ur.UserId, ur.RecipeeId });

            builder.Entity<UserRecipeRating>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.RecipeRatings)
                .HasForeignKey(ur => ur.UserId);

            builder.Entity<UserRecipeRating>()
                .HasOne(ur => ur.Recipee)
                .WithMany(r => r.UserRatings)
                .HasForeignKey(ur => ur.RecipeeId);

            builder.Entity<Comment>()
               .HasOne(c => c.Recipe)
               .WithMany(r => r.Comments)
               .HasForeignKey(c => c.RecipeId);

            builder.Entity<RecipeType>().HasData(
                new RecipeType { Id = 1, Type = "Vegan" },
                new RecipeType { Id = 2, Type = "Vegetarian" },
                new RecipeType { Id = 3, Type = "All" }
            );
            builder.Entity<Product>().HasData(
                new Product { Id = 1, ProductName = "Apple" },
                new Product { Id = 2, ProductName = "Eggs" }
            );
            var userRoleId = "46385bc4-f2f5-4557-a54f-5a555ddbb448";
            var adminRoleId = "322ffb03-c7aa-463f-a4e6-15bbb2da3979";
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "User".ToUpper(),
                    ConcurrencyStamp = userRoleId
                },
                new IdentityRole()
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                    ConcurrencyStamp = adminRoleId
                }
            };
            // Seeding roles
            builder.Entity<IdentityRole>().HasData(roles);
            // Creating admin
            var adminUserId = "fe6995fd-ed28-441f-b4cd-378ec0c046d9";
            var admin = new ApplicationUser()
            {
                Id = adminUserId,
                UserName = "Admin",
                Email = "Admin@gmail.com",
                NormalizedEmail = "admin@gmail.com".ToUpper(),
                NormalizedUserName = "Admin".ToUpper()
            };
            admin.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(admin, "Admin123#");
            builder.Entity<ApplicationUser>().HasData(admin);
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = adminRoleId,
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }

    }
}
