#pragma warning disable CS8618
#pragma warning disable S101

namespace Recipe.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public bool IsVerified { get; set; } = false;
        public ICollection<Recipee>? Recipes { get; set; }
    }
}
