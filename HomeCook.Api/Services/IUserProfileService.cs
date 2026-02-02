using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services
{
    public interface IUserProfileService
    {
        public Task<UserProfileDTO?> GetUserProfileAsync(Guid userId);
        public Task<UserProfileDTO> AddUserProfileAsync(AddUpdateProfileDTO addUpdateProfileDTO);
        public Task<UserProfileDTO?> UpdateUserProfileAsync(AddUpdateProfileDTO updateProfile);
    }
}
