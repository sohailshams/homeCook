using System;
using HomeCook.Api.Models;

namespace HomeCook.Api.EntityFramework.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        var user = await dbContext.Users.FindAsync(userId);
        return user;
    }
}
