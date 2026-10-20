using System.ComponentModel.DataAnnotations;

namespace HomeCook.Api.Dtos
{
    public class AddressRequestDto
    {
        [Required]
        public required string AddressLine1 { get; set; }
        [Required]
        public required string City { get; set; }
        [Required]
        public required string Country { get; set; }
        [Required]
        public required string PostCode { get; set; }
        public bool IsPrimary { get; set; }
    }
}
