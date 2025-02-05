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

        public async Task<Profile?> GetUserProfileByIdAsync(string userId)
        {
            var userProfile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);

            return userProfile;
        }
        public async Task<Profile> AddUserProfileAsync(Profile profile)
        {
             await _dbContext.Profiles.AddAsync(profile);
            await _dbContext.SaveChangesAsync();
            return profile;
        }
    }
}
