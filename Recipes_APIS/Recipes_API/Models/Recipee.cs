using System.ComponentModel.DataAnnotations;

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
    }
}
