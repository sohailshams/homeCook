using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services
{
    public interface IUserService
    {
        public Task<UserInfoDTO?> GetUserInfoAsync();
    }
}
