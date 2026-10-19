using HomeCook.Api.Mappings;
using HomeCook.Api.Dtos;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HomeCook.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly UserManager<User> _userManager;

        public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IUserAddressRepository userAddressRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userAddressRepository = userAddressRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<UserInfoDto> GetUserAndPostCodeInfoAsync()
        {
            try
            {
                var loggedInUserIdString = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");
                var loggedInUserId = Guid.Parse(loggedInUserIdString);
                var user = await _userRepository.GetUserByIdAsync(loggedInUserId);
                if (user == null)
                {
                    throw new NotFoundException($"User with ID {loggedInUserId} was not found.");
                }
                var address = await _userAddressRepository.GetUserAddressListByIdAsync(loggedInUserId);
                var primaryAddress = address.FirstOrDefault(a => a.IsPrimary);

                var userAndPostCodeInfoDto = new UserInfoDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    UserEmail = user.Email,
                    IsProfileComplete = user.IsProfileComplete,
                    PostCode = primaryAddress?.PostCode ?? null,
                    Roles = (await _userManager.GetRolesAsync(user)).ToList()
                };
                return userAndPostCodeInfoDto;

            }
            catch (Exception exception) when (exception is not NotFoundException)
            {
                throw new DatabaseOperationException("An unexpected error occurred while retrieving user profile.", exception);
            }
        }

        public async Task<UserInfoDto?> GetUserInfoAsync()
        {
            try
            {
                var loggedInUserIdString = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");
                var loggedInUserId = Guid.Parse(loggedInUserIdString);
                var user = await _userRepository.GetUserByIdAsync(loggedInUserId) ?? throw new NotFoundException("User not found.");

                // Map user model to UserInfoDto
                var userInfoDto = user.ToInfoDto();
                userInfoDto.Roles = (await _userManager.GetRolesAsync(user)).ToList();

                return userInfoDto;

            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to get a user.", exception);
            }
        }
    }
}
