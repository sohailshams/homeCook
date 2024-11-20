namespace HomeCook.Api.Models
{
    public class Buyer : User
    {
        public List<Food> Foods { get; set; } = new List<Food>();

    }
}
