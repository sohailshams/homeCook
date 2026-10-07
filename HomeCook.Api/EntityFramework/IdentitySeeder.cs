using HomeCook.Api.Constants;
using HomeCook.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeCook.Api.EntityFramework;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        var context = services.GetRequiredService<AppDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = services.GetRequiredService<UserManager<User>>();
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(IdentitySeeder));

        // Create roles
        foreach (var role in Roles.AllRoles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                logger.LogInformation($"Created role {role}");
            }
        }

        var usersWithoutRole = await context.Users
            .Where(u => !context.UserRoles.Any(ur => ur.UserId == u.Id))
            .ToListAsync();

        foreach (var user in usersWithoutRole)
            await userManager.AddToRoleAsync(user, Roles.User);

        // Grant Admin role if account is registered and confirmed
        var adminEmails = configuration.GetSection("Admin:Emails").Get<string[]>() ?? [];

        foreach (var email in adminEmails)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                logger.LogWarning($"Admin email {email} has no registered account; skipping.");
                continue;
            }

            if (!user.EmailConfirmed)
            {
                logger.LogWarning($"Admin email {email} is not confirmed; skipping.");
                continue;
            }

            if (!await userManager.IsInRoleAsync(user, Roles.Admin))
            {
                await userManager.AddToRoleAsync(user, Roles.Admin);
                logger.LogInformation($"Granted {Roles.Admin} role to {email}");
            }
        }
    }
}
