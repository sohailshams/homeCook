namespace HomeCook.Api.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public DateTime AvailableDate { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public Guid RecipeImageId { get; set; }
        public Guid CategoryId { get; set; }
        public List<RecipeImage> RecipeImage { get; set; }
        public Category Category { get; set; }
    }
}
