using HomeCook.Api.Enums;
using Microsoft.AspNetCore.Identity;

namespace HomeCook.Api.Models
{
    public class User : IdentityUser<Guid>
    {
        public bool IsProfileComplete { get; set; } = false;
        public Profile? Profile { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
    }
}
