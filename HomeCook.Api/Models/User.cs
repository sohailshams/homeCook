using HomeCook.Api.Enums;

namespace HomeCook.Api.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public DateTime CreatedAt { get; set; }
         public UserRole Role { get; set; }
        public Profile? Profile { get; set; }

    }
}
