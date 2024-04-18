namespace Recipe.Models.DTO
{
    public class RecipeRatingDTO
    {
        public string UserId { get; set; }
        public int RecipeId { get; set; }
        public float Rating { get; set; }
    }
}
