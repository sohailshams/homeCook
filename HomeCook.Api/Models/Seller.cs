using HomeCook.Api.Enums;

namespace HomeCook.Api.Models
{
    public class Seller : User
    {
        public UserRole Role { get; set; }

    }
}
