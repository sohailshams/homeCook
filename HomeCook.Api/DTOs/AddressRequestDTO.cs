using HomeCook.Api.Models;

namespace HomeCook.Api.DTOs
{
    public class AddressRequestDTO
    {
        public required Guid UserId { get; set; }
        public required string AddressLine1 { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required string PostCode { get; set; }
        public bool IsPrimary { get; set; }
    }
}
