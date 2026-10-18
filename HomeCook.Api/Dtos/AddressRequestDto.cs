using HomeCook.Api.Models;

namespace HomeCook.Api.Dtos
{
    public class AddressRequestDto
    {
        public required Guid UserId { get; set; }
        public required string AddressLine1 { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required string PostCode { get; set; }
        public bool IsPrimary { get; set; }
    }
}
