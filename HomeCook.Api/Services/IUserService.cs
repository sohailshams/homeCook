using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services
{
    public interface IUserService
    {
        public Task<UserInfoDto?> GetUserInfoAsync();
        public Task<UserInfoDto> GetUserAndPostCodeInfoAsync();
    }
}
