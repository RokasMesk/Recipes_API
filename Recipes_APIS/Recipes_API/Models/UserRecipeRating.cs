#pragma warning disable CS8618
#pragma warning disable S101

namespace Recipe.Models
{
    public class UserRecipeRating
    {
        public required string UserId { get; set; }
        public required int RecipeeId { get; set; }

        public float RecipeRating { get; set; } = 0;
        public ApplicationUser User { get; set; }
        public Recipee Recipee { get; set; }

    }
}
