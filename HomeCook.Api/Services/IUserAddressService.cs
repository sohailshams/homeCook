using HomeCook.Api.Dtos;

namespace HomeCook.Api.Services
{
    public interface IUserAddressService
    {
        public Task<AddressRequestDto> AddAddressAsync(AddressRequestDto addressRequestDto);
    }
}
