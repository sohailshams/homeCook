using AutoMapper;
using HomeCook.Api.DTOs;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HomeCook.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IUserRepository userRepository )
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<UserInfoDTO?> GetUserInfoAsync()
        {
            try
            {
                var loggedInUserId = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");

                var user = await _userRepository.GetUserByIdAsync(loggedInUserId) ?? throw new NotFoundException("User not found.");

                //Map user model to UserInfoDTO
                var userInfoDto = _mapper.Map<UserInfoDTO>(user);

                return userInfoDto;

            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to get a user.", exception);
            }
        }
    }
}
