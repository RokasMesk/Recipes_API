#pragma warning disable CS8618
#pragma warning disable S101

namespace Recipe.Models
{
    public class UserFavoriteRecipe
    {
        public string UserId { get; set; }
        public int RecipeeId { get; set; }
        public ApplicationUser User { get; set; }
        public Recipee Recipee { get; set; }
    }
}
