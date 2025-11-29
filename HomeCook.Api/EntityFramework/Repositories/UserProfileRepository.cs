using HomeCook.Api.DTOs;
using HomeCook.Api.Models;
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
        public async Task<Profile> AddUserProfileAsync(Profile profile)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == profile.UserId) ?? throw new Exception($"User with ID {profile.UserId} not found.");
            user.IsProfileComplete = true;
            user.Profile = profile;
            await _dbContext.Profiles.AddAsync(profile);
            await _dbContext.SaveChangesAsync();
            return profile;
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
            existingProfile.Address = updateProfile.Address;
            existingProfile.City = updateProfile.City;
            existingProfile.PostCode = updateProfile.PostCode;
            existingProfile.Country = updateProfile.Country;
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
    }
}
