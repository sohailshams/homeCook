using HomeCook.Api.Models;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public interface IUserAddressRepository
    {
        Task<List<Address>> GetUserAddressListByIdAsync(Guid userId);
        Task<Address> AddUserAddressAsync(Address address);
    }
}
