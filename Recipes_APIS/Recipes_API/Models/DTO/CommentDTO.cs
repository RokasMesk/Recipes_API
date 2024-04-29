namespace Recipe.Models.DTO
{
    public class CommentDTO
    {
        public string UserId { get; set; }
        public int RecipeId { get; set; }
        public string Comment { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
