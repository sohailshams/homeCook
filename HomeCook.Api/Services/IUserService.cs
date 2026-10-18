using HomeCook.Api.Dtos;

namespace HomeCook.Api.Services
{
    public interface IUserService
    {
        public Task<UserInfoDto?> GetUserInfoAsync();
        public Task<UserInfoDto> GetUserAndPostCodeInfoAsync();
    }
}
