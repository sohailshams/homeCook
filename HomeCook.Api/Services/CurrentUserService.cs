using System.Security.Claims;

namespace HomeCook.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly ILogger<CurrentUserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(ILogger<CurrentUserService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetCurrentUserId()
        {
            var loggedInUserIdString = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");
            if (!Guid.TryParse(loggedInUserIdString, out var userId))
            {
                _logger.LogWarning($"Failed to parse user id claim '{loggedInUserIdString}'.");
                throw new UnauthorizedAccessException("Unauthorized user.");
            }

            return userId;
        }
    }
}
