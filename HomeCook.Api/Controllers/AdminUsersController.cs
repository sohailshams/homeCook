using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using HomeCook.Api.Constants;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeCook.Api.Controllers
{
    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class AdminUsersController : ControllerBase
    {
        private readonly ILogger<AdminUsersController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public AdminUsersController(
            ILogger<AdminUsersController> logger,
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("{userId:guid}/roles")]
        public async Task<IActionResult> GetRoles([FromRoute] Guid userId)
        {
            var user = await FindUserAsync(userId);
            return Ok(await _userManager.GetRolesAsync(user));
        }

        [HttpPost("{userId:guid}/roles/{role}")]
        public async Task<IActionResult> AddRole([FromRoute] Guid userId, [FromRoute] string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                throw new ValidationException($"Role '{role}' does not exist.");

            var user = await FindUserAsync(userId);

            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
                throw new ValidationException(string.Join(" ", result.Errors.Select(e => e.Description)));

            // Forces the user's cookie to pick up the new role on the next validation
            await _userManager.UpdateSecurityStampAsync(user);

            _logger.LogInformation($"Admin {GetLoggedInUserId()} granted role {role} to user {userId}");
            return NoContent();
        }

        [HttpDelete("{userId:guid}/roles/{role}")]
        public async Task<IActionResult> RemoveRole([FromRoute] Guid userId, [FromRoute] string role)
        {
            if (string.Equals(role, Roles.Admin, StringComparison.OrdinalIgnoreCase) && userId == GetLoggedInUserId())
                throw new ValidationException("You cannot remove your own admin role.");

            var user = await FindUserAsync(userId);

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
                throw new ValidationException(string.Join(" ", result.Errors.Select(e => e.Description)));

            await _userManager.UpdateSecurityStampAsync(user);

            _logger.LogInformation($"Admin {GetLoggedInUserId()} removed role {role} from user {userId}");
            return NoContent();
        }

        private async Task<User> FindUserAsync(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString())
                   ?? throw new NotFoundException($"User with ID {userId} not found.");
        }

        private Guid GetLoggedInUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var parsedId) ? parsedId : Guid.Empty;
        }
    }
}
