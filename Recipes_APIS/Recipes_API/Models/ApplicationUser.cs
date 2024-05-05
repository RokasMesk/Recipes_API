using Microsoft.AspNetCore.Identity;
#pragma warning disable CS8618
#pragma warning disable S101

namespace Recipe.Models
{
    public class ApplicationUser :IdentityUser
    {
        internal string UserDescription;

        public bool? IsAdmin { get; set; }
        public ICollection<UserFavoriteRecipe> FavoriteRecipes { get; set; }
        public ICollection<UserRecipeRating> RecipeRatings { get; set; }
    }
}
