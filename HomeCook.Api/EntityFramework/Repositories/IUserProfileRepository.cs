using HomeCook.Api.Models;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public interface IUserProfileRepository
    {
        Task<Profile?> GetUserProfileByIdAsync(Guid userId);
        Task<Profile> AddUserProfileAsync(Profile profile);
        Task<Profile?> UpdateUserProfileAsync(string loggedInUserId, Profile updateProfile);
    }
}
