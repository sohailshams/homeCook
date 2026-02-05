using HomeCook.Api.DTOs;
using HomeCook.Api.Models;
using HomeCook.Api.Projections;
using Microsoft.EntityFrameworkCore;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly AppDbContext _dbContext;

        public UserProfileRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Profile?> GetUserProfileByIdAsync(Guid userId)
        {
            var userProfile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);

            return userProfile;
        }

        public async Task<Profile?> UpdateUserProfileAsync(string loggedInUserId, Profile updateProfile)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == updateProfile.UserId) ?? throw new Exception($"User with ID {updateProfile.UserId} not found.");
            var existingProfile = await GetUserProfileByIdAsync(updateProfile.UserId);
            if (existingProfile == null) return null;
            if (existingProfile.UserId.ToString() != loggedInUserId) { throw new UnauthorizedAccessException("You are not authorized to update this profile information."); }
            existingProfile.FirstName = updateProfile.FirstName;
            existingProfile.LastName = updateProfile.LastName;
            existingProfile.PhoneNumber = updateProfile.PhoneNumber;
            existingProfile.User = user;
            existingProfile.UserId = updateProfile.UserId;
            if (updateProfile.ProfileImage != null)
            {
                existingProfile.ProfileImage = updateProfile.ProfileImage;
            }
            if (updateProfile.Bio != null)
            {
                existingProfile.Bio = updateProfile.Bio;
            }

            await _dbContext.SaveChangesAsync();
            return existingProfile;
        }

        public async Task<ProfileWithAddress> AddUserProfileAddressAsync(Profile profile, Address address)
        {

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == profile.UserId);
            if (user == null)
            {
                throw new Exception($"User with ID {profile.UserId} not found.");
            }
            user.IsProfileComplete = true;

            var existingProfile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.UserId == profile.UserId);
            if (existingProfile != null)
            {
                throw new Exception($"A profile with {profile.UserId} already exists.");
            }

            await _dbContext.Profiles.AddAsync(profile);
            await _dbContext.Addresses.AddAsync(address);
            await _dbContext.SaveChangesAsync();
            return new ProfileWithAddress(profile, address);
        }
    }
}
