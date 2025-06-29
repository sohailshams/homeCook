using HomeCook.Api.DTOs;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public interface IUpdateQuantityRepository
    {
        Task<UpdateItemDTO?> UpdateQuantityAsync(Guid foodId, int quantity);
    }
}
