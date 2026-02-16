
using AutoMapper;
using HomeCook.Api.DTOs;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using HomeCook.Api.Projections;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HomeCook.Api.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IMapper _mapper;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IUserAddressRepository _userAddressRepository;

        public UserProfileService(IMapper mapper,
            IUserProfileRepository userProfileRepository,
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IUserAddressRepository userAddressRepository)
        {
            _mapper = mapper;
            _userProfileRepository = userProfileRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _userAddressRepository = userAddressRepository;
        }

        public async Task<UserProfileDTO?> GetUserProfileAsync(Guid userId)
        {
            try
            {
                var userProfileModel = await _userProfileRepository.GetUserProfileByIdAsync(userId);
                if (userProfileModel == null)
                {
                    throw new NotFoundException($"Profile with ID {userId} was not found.");
                }
                var address = await _userAddressRepository.GetUserAddressListByIdAsync(userId);
                var primaryAddress = address.FirstOrDefault(a => a.IsPrimary);
                if (primaryAddress == null)
                {
                    throw new NotFoundException($"Address with ID {userId} was not found.");
                }
                var userProfileDTO = new UserProfileDTO
                {
                    UserId = userProfileModel.UserId,
                    FirstName = userProfileModel.FirstName,
                    LastName = userProfileModel.LastName,
                    PhoneNumber = userProfileModel.PhoneNumber,
                    Bio = userProfileModel.Bio,
                    ProfileImage = userProfileModel.ProfileImage,
                    City = primaryAddress.City,
                    Country = primaryAddress.Country,
                    AddressLine1 = primaryAddress.AddressLine1,
                    PostCode = primaryAddress.PostCode,
                    IsPrimary = primaryAddress.IsPrimary
                };
                return userProfileDTO;

            }
            catch (Exception exception) when (exception is not NotFoundException)
            {
                throw new DatabaseOperationException("An unexpected error occurred while retrieving user profile.", exception);
            }
        }

        public async Task<UserProfileDTO> AddUserProfileAsync(AddUpdateProfileDTO addUpdateProfileDTO)
        {
            try
            {
                var loggedInUserIdString = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");
                var loggedInUserId = Guid.Parse(loggedInUserIdString);
                if (loggedInUserId != addUpdateProfileDTO.UserId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to add profile information in database.");
                }

                var user = await _userRepository.GetUserByIdAsync(addUpdateProfileDTO.UserId) ?? throw new NotFoundException($"User with ID {addUpdateProfileDTO.UserId} not found.");

                // Create Address model from AddUpdateProfileDTO
                var addressModel = new Address
                {
                    City = addUpdateProfileDTO.City,
                    Country = addUpdateProfileDTO.Country,
                    IsPrimary = true,
                    AddressLine1 = addUpdateProfileDTO.AddressLine1,
                    PostCode = addUpdateProfileDTO.PostCode,
                    User = user,
                    UserId = addUpdateProfileDTO.UserId
                };

                // Create Profile model from AddUpdateProfileDTO
                var profileModel = new HomeCook.Api.Models.Profile
                {
                    UserId = addUpdateProfileDTO.UserId,
                    FirstName = addUpdateProfileDTO.FirstName,
                    LastName = addUpdateProfileDTO.LastName,
                    PhoneNumber = addUpdateProfileDTO.PhoneNumber,
                    User = user,
                    Bio = addUpdateProfileDTO.Bio,
                    ProfileImage = addUpdateProfileDTO.ProfileImage
                };

                var profileWithAddress = await _userProfileRepository.AddUserProfileAddressAsync(profileModel, addressModel);


                var userProfileDto = new UserProfileDTO
                {
                    UserId = profileWithAddress.Profile.UserId,
                    FirstName = profileWithAddress.Profile.FirstName,
                    LastName = profileWithAddress.Profile.LastName,
                    PhoneNumber = profileWithAddress.Profile.PhoneNumber,
                    Bio = profileWithAddress.Profile.Bio,
                    ProfileImage = profileWithAddress.Profile.ProfileImage,
                    City = profileWithAddress.Address.City,
                    Country = profileWithAddress.Address.Country,
                    AddressLine1 = profileWithAddress.Address.AddressLine1,
                    PostCode = profileWithAddress.Address.PostCode,
                    IsPrimary = profileWithAddress.Address.IsPrimary
                };

                return userProfileDto;
            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to add profile information in database.", exception);
            }
        }

        public async Task<UserProfileDTO?> UpdateUserProfileAsync(AddUpdateProfileDTO updateProfile)
        {
            try
            {
                var loggedInUserId = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");

                // Convert AddUpdateProfileDTO to model
                var profileModal = _mapper.Map<Models.Profile>(updateProfile);
                var updatedProfile = await _userProfileRepository.UpdateUserProfileAsync(loggedInUserId, profileModal) ?? throw new NotFoundException("Profile informaton not found.");

                // Convert Profile model to UserProfileDTO
                var updatedProfileDto = _mapper.Map<UserProfileDTO>(updatedProfile);

                return updatedProfileDto;
            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to update profile information.", exception);
            }
        }
    }
}
