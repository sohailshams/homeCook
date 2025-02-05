
using AutoMapper;
using HomeCook.Api.DTOs;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
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

        public UserProfileService(IMapper mapper, IUserProfileRepository userProfileRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userProfileRepository = userProfileRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<UserProfileDTO?> GetUserProfileAsync(string userId)
        {
            try
            {
                var userProfileModel = await _userProfileRepository.GetUserProfileByIdAsync(userId);
               if (userProfileModel == null)
                {
                    throw new NotFoundException($"Profile with ID {userId} was not found.");
                }
                var userProfileDTO = _mapper.Map<UserProfileDTO>(userProfileModel);
                return userProfileDTO;
                 
            }
            catch (Exception exception) when (exception is not NotFoundException)
            {
                throw new DatabaseOperationException("An unexpected error occurred while retrieving user profile.", exception);
            }
        }

        public async Task<AddUpdateProfileDTO> AddUserProfileAsync(AddUpdateProfileDTO addUpdateProfileDTO)
        {
            try 
            { 
                var loggedInUserId = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");

                if (loggedInUserId != addUpdateProfileDTO.UserId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to add profile information in database.");
                }

                // Convert AddUpdateProfileDTO to model
                var profileModel = _mapper.Map<Models.Profile>(addUpdateProfileDTO);
                var user = await _userRepository.GetUserByIdAsync(profileModel.UserId) ?? throw new NotFoundException($"User with ID {profileModel.UserId} not found.");
                profileModel.User = user;

                var newProfile = await _userProfileRepository.AddUserProfileAsync(profileModel);

                // Map model to UserPforileDTO
                var userProfileDto = _mapper.Map<AddUpdateProfileDTO>(newProfile);

                return userProfileDto;
            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to add profile information in database.", exception);
            }
        }
    }
}
