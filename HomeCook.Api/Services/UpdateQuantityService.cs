
using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services
{
    public class UpdateQuantityService : IUpdateQuantityService
    {
        public UpdateQuantityService()
        {
        }

        public Task<UpdateItemDTO> UpdateQuantityAsync(Guid foodId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
