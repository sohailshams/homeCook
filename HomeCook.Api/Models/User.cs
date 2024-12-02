using HomeCook.Api.Enums;
using Microsoft.AspNetCore.Identity;

namespace HomeCook.Api.Models
{
    public class User : IdentityUser
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserRole Role { get; set; }
        public Profile? Profile { get; set; }

    }
}
