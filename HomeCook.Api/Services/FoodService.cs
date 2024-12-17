using AutoMapper;
using HomeCook.Api.DTOs;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HomeCook.Api.Services
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;
        public readonly ICategoryService _categoryService;
        public readonly IUserRepository _userRepository;
        private readonly ICategoryReposity _categoryReposity;

        public FoodService(IFoodRepository foodRepository, IMapper mapper, ICategoryService categoryService, IUserRepository userRepository, ICategoryReposity categoryReposity)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
            _categoryService = categoryService;
            _userRepository = userRepository;
            _categoryReposity = categoryReposity;
        }

        public async Task<List<FoodDTO>> GetFoodListAsync()
        {
            // get food list from db
            var food = await _foodRepository.GetFoodListAsync();

            // map food list<Food> to food list<FoodDTO>
            var foodDto = _mapper.Map<List<FoodDTO>>(food);
            return foodDto;
        }

        public async Task<FoodDTO> AddFoodAsync(AddFoodDTO addFoodDTO)
        {
            try
            {
                // TODO: Remember to check if logged in user is the same as seller

                // Convert addFoodDTO to model
                var foodModel = _mapper.Map<Food>(addFoodDTO);

                var category = await _categoryReposity.GetCategoryByIdAsync(foodModel.CategoryId) ?? throw new ValidationException($"Category with ID {addFoodDTO.CategoryId} not found.");
                foodModel.Category = category;

                var seller = await _userRepository.GetUserByIdAsync(foodModel.SellerId) ?? throw new KeyNotFoundException($"Seller with ID {addFoodDTO.SellerId} not found.");
                foodModel.Seller = seller;
                
                // Use model to create food object in DB
                var newFood = await _foodRepository.AddFoodAsync(foodModel);

                //Map model to DTO
                var foodDto = _mapper.Map<FoodDTO>(newFood);

                return foodDto;

            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to create food.", exception);
            }
        }
    }
}
