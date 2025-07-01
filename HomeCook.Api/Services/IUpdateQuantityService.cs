namespace HomeCook.Api.Services
{
    public interface IUpdateQuantityService
    {
        public Task<bool> UpdateQuantityAsync(Guid foodId, int quantity);
    }
}
