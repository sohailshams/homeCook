namespace HomeCook.Api.Models
{
    public class Profile
    {
        public Guid ProfileId { get; set; }
        public required Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImage { get; set; }
    }
}
