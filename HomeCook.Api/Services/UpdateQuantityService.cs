
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

        public async Task<bool> UpdateQuantityAsync(Guid foodId, int quantity)
        {
            try
            {
                var result = await _updateQuantityRepository.UpdateQuantityAsync(foodId, quantity);
                if (result == null)
                {
                    return false;
                }

                return true;

            }
            catch (Exception exception)
            {
                return false;
            }
        }
    }
}
