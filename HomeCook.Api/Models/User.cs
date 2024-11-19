namespace HomeCook.Api.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid ProfileId { get; set; }
        public Profile? Profile { get; set; }

    }
}
