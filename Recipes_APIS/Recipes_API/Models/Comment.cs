using System.ComponentModel.DataAnnotations;

namespace Recipe.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime PostedAt { get; set; }

        // Foreign Key for User
        public string UserId { get; set; }
        // Navigation property for User
        public ApplicationUser User { get; set; }

        // Foreign Key for Recipe
        public int RecipeId { get; set; }
        // Navigation property for Recipe
        public Recipee Recipe { get; set; }
        public string AuthorName { get; set; }
    }
}
