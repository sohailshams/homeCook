using HomeCook.Api.Enums;

namespace HomeCook.Api.Models
{
    public class Buyer : User
    {
        public Guid Id { get; set; }
        public UserRole Role { get; set; }

    }
}
