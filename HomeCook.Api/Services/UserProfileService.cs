using HomeCook.Api.Dtos;
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
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly IPostcodeService _postcodeService;

        public UserProfileService(IUserProfileRepository userProfileRepository,
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IUserAddressRepository userAddressRepository,
            IPostcodeService postcodeService)
        {
            _userProfileRepository = userProfileRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _userAddressRepository = userAddressRepository;
            _postcodeService = postcodeService;
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(Guid userId)
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
                var userProfileDto = new UserProfileDto
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
                return userProfileDto;

            }
            catch (Exception exception) when (exception is not NotFoundException)
            {
                throw new DatabaseOperationException("An unexpected error occurred while retrieving user profile.", exception);
            }
        }

        public async Task<UserProfileDto> AddUserProfileAsync(AddUpdateProfileDto addUpdateProfileDto)
        {
            try
            {
                var loggedInUserIdString = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");
                var loggedInUserId = Guid.Parse(loggedInUserIdString);
                if (loggedInUserId != addUpdateProfileDto.UserId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to add profile information in database.");
                }

                var user = await _userRepository.GetUserByIdAsync(addUpdateProfileDto.UserId) ?? throw new NotFoundException($"User with ID {addUpdateProfileDto.UserId} not found.");

                // Create Address model from AddUpdateProfileDto
                var addressModel = new Address
                {
                    City = addUpdateProfileDto.City,
                    Country = addUpdateProfileDto.Country,
                    IsPrimary = true,
                    AddressLine1 = addUpdateProfileDto.AddressLine1,
                    PostCode = addUpdateProfileDto.PostCode,
                    User = user,
                    UserId = addUpdateProfileDto.UserId,
                    Location = await _postcodeService.GetLocationAsync(addUpdateProfileDto.PostCode)
                };

                // Create Profile model from AddUpdateProfileDto
                var profileModel = new HomeCook.Api.Models.Profile
                {
                    UserId = addUpdateProfileDto.UserId,
                    FirstName = addUpdateProfileDto.FirstName,
                    LastName = addUpdateProfileDto.LastName,
                    PhoneNumber = addUpdateProfileDto.PhoneNumber,
                    User = user,
                    Bio = addUpdateProfileDto.Bio,
                    ProfileImage = addUpdateProfileDto.ProfileImage
                };

                var profileWithAddress = await _userProfileRepository.AddUserProfileAddressAsync(profileModel, addressModel);


                var userProfileDto = new UserProfileDto
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

        public async Task<UserProfileDto?> UpdateUserProfileAsync(AddUpdateProfileDto updateProfile)
        {
            try
            {
                var loggedInUserIdString = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");
                var loggedInUserId = Guid.Parse(loggedInUserIdString);
                if (loggedInUserId != updateProfile.UserId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to update profile information in database.");
                }

                var existingProfile = await _userProfileRepository.GetUserProfileByIdAsync(updateProfile.UserId);
                if (existingProfile == null)
                {
                    throw new NotFoundException($"Profile with ID {updateProfile.UserId} was not found.");
                }

                var addressList = await _userAddressRepository.GetUserAddressListByIdAsync(updateProfile.UserId);
                var primaryAddress = addressList.FirstOrDefault(a => a.IsPrimary);

                var user = await _userRepository.GetUserByIdAsync(updateProfile.UserId) ?? throw new NotFoundException($"User with ID {updateProfile.UserId} not found.");

                // update Address model
                primaryAddress.City = updateProfile.City;
                primaryAddress.Country = updateProfile.Country;
                primaryAddress.IsPrimary = true;
                primaryAddress.AddressLine1 = updateProfile.AddressLine1;
                primaryAddress.PostCode = updateProfile.PostCode;
                primaryAddress.User = user;
                primaryAddress.UserId = updateProfile.UserId;
                primaryAddress.Location = await _postcodeService.GetLocationAsync(updateProfile.PostCode);

                // update Profile model
                existingProfile.UserId = updateProfile.UserId;
                existingProfile.FirstName = updateProfile.FirstName;
                existingProfile.LastName = updateProfile.LastName;
                existingProfile.PhoneNumber = updateProfile.PhoneNumber;
                existingProfile.User = user;
                existingProfile.Bio = updateProfile.Bio;
                existingProfile.ProfileImage = updateProfile.ProfileImage;

                var profileWithAddress = await _userProfileRepository.UpdateUserProfileAddressAsync(existingProfile, primaryAddress);


                var userProfileDto = new UserProfileDto
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
                throw new DatabaseOperationException("Failed to update profile information.", exception);
            }
        }
    }
}
