using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services
{
    public interface IUpdateQuantityService
    {
        public Task<UpdateItemDTO> UpdateQuantityAsync(Guid foodId, int quantity);
    }
}
