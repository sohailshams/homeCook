using HomeCook.Api.Dtos;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Mappings;
using Microsoft.EntityFrameworkCore;

namespace HomeCook.Api.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly IPostcodeService _postcodeService;

        public UserProfileService(IUserProfileRepository userProfileRepository,
            ICurrentUserService currentUserService,
            IUserRepository userRepository,
            IUserAddressRepository userAddressRepository,
            IPostcodeService postcodeService)
        {
            _userProfileRepository = userProfileRepository;
            _currentUserService = currentUserService;
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
                var primaryAddress = address.FirstOrDefault(a => a.IsPrimary)
                    ?? throw new NotFoundException($"Primary address for user {userId} was not found.");

                return userProfileModel.ToDto(primaryAddress);
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
                var loggedInUserId = _currentUserService.GetCurrentUserId();

                _ = await _userRepository.GetUserByIdAsync(loggedInUserId) ?? throw new NotFoundException($"User with ID {loggedInUserId} not found.");

                var location = await _postcodeService.GetLocationAsync(addUpdateProfileDto.PostCode);
                var addressModel = addUpdateProfileDto.ToAddressModel(loggedInUserId, location);
                var profileModel = addUpdateProfileDto.ToProfileModel(loggedInUserId);

                var profileWithAddress = await _userProfileRepository.AddUserProfileAddressAsync(profileModel, addressModel);

                return profileWithAddress.Profile.ToDto(profileWithAddress.Address);
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
                var loggedInUserId = _currentUserService.GetCurrentUserId();

                var existingProfile = await _userProfileRepository.GetUserProfileByIdAsync(loggedInUserId)
                    ?? throw new NotFoundException($"Profile with ID {loggedInUserId} was not found.");

                var addressList = await _userAddressRepository.GetUserAddressListByIdAsync(loggedInUserId);
                var primaryAddress = addressList.FirstOrDefault(a => a.IsPrimary)
                    ?? throw new NotFoundException($"Primary address for user {loggedInUserId} was not found.");

                // Copy only the fields a user may change - ids and ownership stay as they are
                primaryAddress.AddressLine1 = updateProfile.AddressLine1;
                primaryAddress.City = updateProfile.City;
                primaryAddress.Country = updateProfile.Country;
                primaryAddress.PostCode = updateProfile.PostCode;
                primaryAddress.IsPrimary = true;
                primaryAddress.Location = await _postcodeService.GetLocationAsync(updateProfile.PostCode);

                existingProfile.FirstName = updateProfile.FirstName;
                existingProfile.LastName = updateProfile.LastName;
                existingProfile.PhoneNumber = updateProfile.PhoneNumber;
                existingProfile.Bio = updateProfile.Bio;
                existingProfile.ProfileImage = updateProfile.ProfileImage;

                var profileWithAddress = await _userProfileRepository.UpdateUserProfileAddressAsync(existingProfile, primaryAddress);

                return profileWithAddress.Profile.ToDto(profileWithAddress.Address);
            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to update profile information.", exception);
            }
        }
    }
}
