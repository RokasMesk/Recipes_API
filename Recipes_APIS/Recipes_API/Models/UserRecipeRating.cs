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
