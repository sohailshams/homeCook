using HomeCook.Api.Dtos;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Mappings;
using Microsoft.EntityFrameworkCore;

namespace HomeCook.Api.Services
{
    public class UserAddressService : IUserAddressService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly IPostcodeService _postcodeService;

        public UserAddressService(ICurrentUserService currentUserService,
            IUserRepository userRepository,
            IUserAddressRepository userAddressRepository,
            IPostcodeService postcodeService)
        {
            _currentUserService = currentUserService;
            _userRepository = userRepository;
            _userAddressRepository = userAddressRepository;
            _postcodeService = postcodeService;
        }

        public async Task<AddressDto> AddAddressAsync(AddressRequestDto addressRequestDto)
        {
            try
            {
                var loggedInUserId = _currentUserService.GetCurrentUserId();

                _ = await _userRepository.GetUserByIdAsync(loggedInUserId) ?? throw new NotFoundException($"User with ID {loggedInUserId} not found.");

                var location = await _postcodeService.GetLocationAsync(addressRequestDto.PostCode);

                var address = addressRequestDto.ToModel(loggedInUserId, location);

                var savedAddress = await _userAddressRepository.AddUserAddressAsync(address);

                return savedAddress.ToDto();
            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to add address in database.", exception);
            }
        }
    }
}
