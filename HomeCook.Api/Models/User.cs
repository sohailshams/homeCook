using HomeCook.Api.Enums;
using Microsoft.AspNetCore.Identity;

namespace HomeCook.Api.Models
{
    public class User : IdentityUser
    {
        //public Guid Id { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Email { get; set; }
        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserRole Role { get; set; }
        public Profile? Profile { get; set; }

        //public User()
        //{
        //    FirstName = string.Empty;
        //    LastName = string.Empty;
        //    Email = string.Empty;
        //}
    }
}
