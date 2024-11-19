namespace HomeCook.Api.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public DateTime AvailableDate { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public required string Description { get; set; }
        public required string Ingredients { get; set; }
        public Guid RecipeImageId { get; set; }
        public Guid CategoryId { get; set; }
        public List<RecipeImage>? RecipeImage { get; set; }
        public required Category Category { get; set; }
    }
}
