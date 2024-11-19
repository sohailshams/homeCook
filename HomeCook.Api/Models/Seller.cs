using HomeCook.Api.Enums;

namespace HomeCook.Api.Models
{
    public class Seller : User
    {
        public Guid Id { get; set; }
        public UserRole Role { get; set; }

    }
}
