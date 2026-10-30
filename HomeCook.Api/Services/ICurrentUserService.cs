using HomeCook.Api.Dtos;

namespace HomeCook.Api.Services
{
    public interface ICurrentUserService
    {
        public Guid GetCurrentUserId();
    }
}
