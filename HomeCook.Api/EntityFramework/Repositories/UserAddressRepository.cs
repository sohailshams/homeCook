using HomeCook.Api.DTOs;
using HomeCook.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public class UserAddressRepository : IUserAddressRepository
    {
        private readonly AppDbContext _dbContext;

        public UserAddressRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Address>> GetUserAddressListByIdAsync(Guid userId)
        {
            var userAddressList = await _dbContext.Addresses.Where(p => p.UserId == userId).ToListAsync();

            return userAddressList;
        }
        public async Task<Address> AddUserAddressAsync(Address address)
        {
            if (address.IsPrimary)
            {
                var existingAddress = await _dbContext.Addresses.FirstOrDefaultAsync(a => a.UserId == address.UserId && a.IsPrimary);
                if (existingAddress != null)
                {
                    existingAddress.IsPrimary = false;
                }
            }

            await _dbContext.Addresses.AddAsync(address);
            await _dbContext.SaveChangesAsync();
            return address;
        }
    }
}
