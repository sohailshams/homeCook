namespace HomeCook.Api.DTOs
{
    public class UserInfoDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public bool IsProfileComplete { get; set; }
    }
}
