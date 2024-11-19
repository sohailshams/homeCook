using HomeCook.Api.Enums;

namespace HomeCook.Api.Models
{
    public class Buyer : User
    {
        public UserRole Role { get; set; }

    }
}
