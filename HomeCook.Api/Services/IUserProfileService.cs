using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services
{
    public interface IUserProfileService
    {
        public Task<UserProfileDTO?> GetUserProfileAsync(string userId);
    }
}
