using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618
#pragma warning disable S101

namespace Recipe.Models
{
    public class Recipee
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Product> Products { get; set; }
        public string? Preparation { get; set; }
        public string? SkillLevel { get; set; }
        public int? TimeForCooking { get; set; }
        public RecipeType Type { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<UserFavoriteRecipe> UserFavorites { get; set; }
        public double Rating { get; set; } = 0;
        public int RatedPeopleCount { get; set; } = 0;
        public ICollection<UserRecipeRating> UserRatings { get; set; } = new List<UserRecipeRating>();
        public ICollection<Comment> Comments { get; set; }
    }
}
