using HomeCook.Api.DTOs;
using HomeCook.Api.Models;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public interface IUserProfileRepository
    {
        Task<Profile?> GetUserProfileByIdAsync(string userId);
    }
}
