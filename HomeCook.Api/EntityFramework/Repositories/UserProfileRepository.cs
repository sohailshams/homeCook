using HomeCook.Api.Dtos;
using HomeCook.Api.Exceptions;
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

        public async Task<ProfileWithAddress> AddUserProfileAddressAsync(Profile profile, Address address)
        {

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == profile.UserId);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {profile.UserId} not found.");
            }
            user.IsProfileComplete = true;

            var existingProfile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.UserId == profile.UserId);
            if (existingProfile != null)
            {
                throw new EntityAlreadyExistsException($"A profile with {profile.UserId} already exists.");
            }

            await _dbContext.Profiles.AddAsync(profile);
            await _dbContext.Addresses.AddAsync(address);
            await _dbContext.SaveChangesAsync();
            return new ProfileWithAddress(profile, address);
        }
        public async Task<ProfileWithAddress> UpdateUserProfileAddressAsync(Profile profile, Address address)
        {
            await _dbContext.SaveChangesAsync();
            return new ProfileWithAddress(profile, address);
        }
    }
}
