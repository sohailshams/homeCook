namespace HomeCook.Api.Models
{
    public class Profile
    {
        public Guid Id { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required string PostCode { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImage { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; }
    }
}
