namespace Recipe.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public ICollection<Recipee>? Recipes { get; set; }
    }
}
