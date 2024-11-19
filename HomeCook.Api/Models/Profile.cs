namespace HomeCook.Api.Models
{
    public class Profile
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImage { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
