using Microsoft.EntityFrameworkCore;
using Recipe.Models;

namespace Recipe.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Recipee> Recipes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<RecipeType> RecipeTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RecipeType>().HasData(
            new RecipeType { Id = 1, Type = "Vegan" },
             new RecipeType { Id = 2, Type = "Vegetarian" },
              new RecipeType { Id = 3, Type = "All" }
            );
        }
    }
}
