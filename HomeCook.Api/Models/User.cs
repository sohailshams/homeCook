namespace HomeCook.Api.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }

    }
}
