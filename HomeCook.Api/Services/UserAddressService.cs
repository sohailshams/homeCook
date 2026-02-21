
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

        public async Task<AddressRequestDTO> AddAddressAsync(AddressRequestDTO addressRequestDTO)
        {
            try
            {
                var loggedInUserIdString = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");
                var loggedInUserId = Guid.Parse(loggedInUserIdString);
                if (loggedInUserId != addressRequestDTO.UserId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to add address in database.");
                }

                var user = await _userRepository.GetUserByIdAsync(addressRequestDTO.UserId) ?? throw new NotFoundException($"User with ID {addressRequestDTO.UserId} not found.");

                // Create Address model from AddressRequestDTO
                var addressModel = new Address
                {
                    City = addressRequestDTO.City,
                    Country = addressRequestDTO.Country,
                    IsPrimary = addressRequestDTO.IsPrimary,
                    AddressLine1 = addressRequestDTO.AddressLine1,
                    PostCode = addressRequestDTO.PostCode,
                    User = user,
                    UserId = addressRequestDTO.UserId
                };

                var userAddress = await _userAddressRepository.AddUserAddressAsync(addressModel);

                var addressResponseDTO = new AddressRequestDTO
                {
                    UserId = userAddress.UserId,
                    City = userAddress.City,
                    Country = userAddress.Country,
                    IsPrimary = userAddress.IsPrimary,
                    AddressLine1 = userAddress.AddressLine1,
                    PostCode = userAddress.PostCode
                };

                return addressResponseDTO;
            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to add profile information in database.", exception);
            }
        }
    }
}
