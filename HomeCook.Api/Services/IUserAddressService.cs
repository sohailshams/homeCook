using HomeCook.Api.Dtos;

namespace HomeCook.Api.Services
{
    public interface IUserAddressService
    {
        public Task<AddressDto> AddAddressAsync(AddressRequestDto addressRequestDto);
    }
}
