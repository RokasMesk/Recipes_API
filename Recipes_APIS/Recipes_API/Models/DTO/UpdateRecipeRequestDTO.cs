namespace Recipe.Models.DTO
{
    public class UpdateRecipeRequestDTO
    {
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<int> Products { get; set; } = new List<int>();
        public string? Preparation { get; set; }
        public string? SkillLevel { get; set; }
        public int? TimeForCooking { get; set; }
        public int Type { get; set; } 
    }
}
