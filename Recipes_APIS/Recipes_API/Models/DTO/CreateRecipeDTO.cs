namespace Recipe.Models.DTO
{
    public class CreateRecipeDTO
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int[] Products { get; set; }
        public string Preparation { get; set; }
        public string SkillLevel { get; set; }
        public int TimeForCooking { get; set; }
        public int Type { get; set; }
    }
}
