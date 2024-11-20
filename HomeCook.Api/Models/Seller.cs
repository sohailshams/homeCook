namespace HomeCook.Api.Models
{
    public class Seller : User
    {
    public List<Food> Foods { get; set; } = new List<Food>();
    }
}
