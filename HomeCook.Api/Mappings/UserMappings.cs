using HomeCook.Api.Dtos;
using HomeCook.Api.Models;

namespace HomeCook.Api.Mappings
{
    public static class UserMappings
    {
        public static UserInfoDto ToInfoDto(this User user) => new()
        {
            Id = user.Id,
            UserName = user.UserName!,
            UserEmail = user.Email!,
            IsProfileComplete = user.IsProfileComplete
        };
    }
}
