using HomeCook.Api.Dtos;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HomeCook.Api.Services
{
    public class UserAddressService : IUserAddressService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly IPostcodeService _postcodeService;

        public UserAddressService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IUserAddressRepository userAddressRepository,
            IPostcodeService postcodeService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _userAddressRepository = userAddressRepository;
            _postcodeService = postcodeService;
        }

        public async Task<AddressDto> AddAddressAsync(AddressRequestDto addressRequestDto)
        {
            try
            {
                var loggedInUserIdString = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");
                if (!Guid.TryParse(loggedInUserIdString, out var loggedInUserId))
                {
                    throw new UnauthorizedAccessException("Unauthorized user.");
                }

                var user = await _userRepository.GetUserByIdAsync(loggedInUserId) ?? throw new NotFoundException($"User with ID {loggedInUserId} not found.");

                if (user.Id != loggedInUserId)
                {
                    throw new UnauthorizedAccessException("Unauthorized user.");
                }

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
