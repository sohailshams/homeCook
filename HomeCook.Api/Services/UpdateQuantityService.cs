
using HomeCook.Api.DTOs;
using HomeCook.Api.EntityFramework.Repositories;

namespace HomeCook.Api.Services
{
    public class UpdateQuantityService : IUpdateQuantityService
    {
        private readonly IUpdateQuantityRepository _updateQuantityRepository;

        public UpdateQuantityService(IUpdateQuantityRepository updateQuantityRepository)
        {
            _updateQuantityRepository = updateQuantityRepository;
        }

        public async Task<UpdateItemDTO> UpdateQuantityAsync(Guid foodId, int quantity)
        {
            return await _updateQuantityRepository.UpdateQuantityAsync(foodId, quantity);
        }
    }
}
