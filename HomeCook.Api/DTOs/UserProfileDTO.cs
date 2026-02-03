using HomeCook.Api.Models;

namespace HomeCook.Api.DTOs
{
    public class UserProfileDTO
    {
        public Guid ProfileId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string AddressLine1 { get; set; }
        public required string PhoneNumber { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required string PostCode { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImage { get; set; }
        public required Guid UserId { get; set; }
        public bool IsPrimary { get; set; }
    }
}
