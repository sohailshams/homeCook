
using AutoMapper;
using HomeCook.Api.DTOs;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HomeCook.Api.Services
{
    public class UserAddressService : IUserAddressService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IUserAddressRepository _userAddressRepository;

        public UserAddressService(IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IUserAddressRepository userAddressRepository)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _userAddressRepository = userAddressRepository;
        }

        public async Task<AddressRequestDto> AddAddressAsync(AddressRequestDto addressRequestDto)
        {
            try
            {
                var loggedInUserIdString = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");
                var loggedInUserId = Guid.Parse(loggedInUserIdString);
                if (loggedInUserId != addressRequestDto.UserId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to add address in database.");
                }

                var user = await _userRepository.GetUserByIdAsync(addressRequestDto.UserId) ?? throw new NotFoundException($"User with ID {addressRequestDto.UserId} not found.");

                // Create Address model from AddressRequestDto
                var addressModel = new Address
                {
                    City = addressRequestDto.City,
                    Country = addressRequestDto.Country,
                    IsPrimary = addressRequestDto.IsPrimary,
                    AddressLine1 = addressRequestDto.AddressLine1,
                    PostCode = addressRequestDto.PostCode,
                    User = user,
                    UserId = addressRequestDto.UserId
                };

                var userAddress = await _userAddressRepository.AddUserAddressAsync(addressModel);

                var addressResponseDto = new AddressRequestDto
                {
                    UserId = userAddress.UserId,
                    City = userAddress.City,
                    Country = userAddress.Country,
                    IsPrimary = userAddress.IsPrimary,
                    AddressLine1 = userAddress.AddressLine1,
                    PostCode = userAddress.PostCode
                };

                return addressResponseDto;
            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to add profile information in database.", exception);
            }
        }
    }
}
