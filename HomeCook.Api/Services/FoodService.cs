using AutoMapper;
using HomeCook.Api.DTOs;
using HomeCook.Api.EntityFramework.Repositories;

namespace HomeCook.Api.Services
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;
        public FoodService(IFoodRepository foodRepository, IMapper mapper)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;

        }

        public async Task<List<FoodDTO>> GetFoodListAsync()
        {
            // get food list from db
            var food = await _foodRepository.GetFoodListAsync();

            // map food list<Food> to food list<FoodDTO>
            var foodDto = _mapper.Map<List<FoodDTO>>(food);
            return foodDto;
        }
    }
}
