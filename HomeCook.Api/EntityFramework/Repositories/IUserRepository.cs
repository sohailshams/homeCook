using HomeCook.Api.Models;

namespace HomeCook.Api.EntityFramework.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid userId);

}
