using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services
{
    public interface IUserAddressService
    {
        public Task<AddressRequestDTO> AddAddressAsync(AddressRequestDTO addressRequestDTO);
    }
}
