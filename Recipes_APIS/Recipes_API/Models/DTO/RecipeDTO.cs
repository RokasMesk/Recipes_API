namespace Recipe.Models.DTO
{
    public class RecipeDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
        public string? Preparation { get; set; }
        public string? SkillLevel { get; set; }
        public int? TimeForCooking { get; set; }
        public RecipeType Type { get; set; }
        public string? RecipeCreatorUserName { get; set; }
        public double Rating { get; set; }
        public int RatedPeopleCount { get; set; }
    }
}
