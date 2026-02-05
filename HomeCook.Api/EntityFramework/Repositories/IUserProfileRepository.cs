using HomeCook.Api.Models;
using HomeCook.Api.Projections;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public interface IUserProfileRepository
    {
        Task<Profile?> GetUserProfileByIdAsync(Guid userId);
        Task<ProfileWithAddress> AddUserProfileAddressAsync(Profile profile, Address address);
        Task<Profile?> UpdateUserProfileAsync(string loggedInUserId, Profile updateProfile);
    }
}
