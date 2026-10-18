using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services
{
    public interface IUserProfileService
    {
        public Task<UserProfileDto?> GetUserProfileAsync(Guid userId);
        public Task<UserProfileDto> AddUserProfileAsync(AddUpdateProfileDto addUpdateProfileDto);
        public Task<UserProfileDto?> UpdateUserProfileAsync(AddUpdateProfileDto updateProfile);
    }
}
