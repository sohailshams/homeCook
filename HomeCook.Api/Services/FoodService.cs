using AutoMapper;
using HomeCook.Api.DTOs;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace HomeCook.Api.Services
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;
        public readonly ICategoryService _categoryService;
        public readonly IUserRepository _userRepository;
        private readonly ICategoryReposity _categoryReposity;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FoodService(IFoodRepository foodRepository, IMapper mapper, ICategoryService categoryService, IUserRepository userRepository, ICategoryReposity categoryReposity, IHttpContextAccessor httpContextAccessor)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
            _categoryService = categoryService;
            _userRepository = userRepository;
            _categoryReposity = categoryReposity;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<FoodDTO>> GetFoodListAsync()
        {
            // get food list from db
            var food = await _foodRepository.GetFoodListAsync();

            // map food list<Food> to food list<FoodDTO>
            var foodDto = _mapper.Map<List<FoodDTO>>(food);
            return foodDto;
        }

        public async Task<FoodDetailDTO?> GetFoodDetailAsync(Guid id)
        {
            try
            {
                var foodDetail = await _foodRepository.GetFoodDetailAsync(id) ?? throw new NotFoundException("Food not found.");
                // map Food model to FoodDetailDTO
                var foodDetailDTO = _mapper.Map<FoodDetailDTO>(foodDetail);
                return foodDetailDTO;

            } catch (Exception exception) when (exception is not NotFoundException)
            {
                throw new DatabaseOperationException("An unexpected error occurred while retrieving food details.", exception);
            }
        }

        public async Task<FoodDTO> AddFoodAsync(AddUpdateFoodDTO addFoodDTO)
        {
            try
            {
                var loggedInUserId = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");

                if (loggedInUserId != addFoodDTO.SellerId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to create a food.");
                }

                // Convert addFoodDTO to model
                var foodModel = _mapper.Map<Food>(addFoodDTO);

                var category = await _categoryReposity.GetCategoryByIdAsync(foodModel.CategoryId) ?? throw new NotFoundException($"Category with ID {addFoodDTO.CategoryId} not found.");
                foodModel.Category = category;

                var seller = await _userRepository.GetUserByIdAsync(foodModel.SellerId) ?? throw new NotFoundException($"Seller with ID {addFoodDTO.SellerId} not found.");
                foodModel.Seller = seller;

                var foodImageUrls = addFoodDTO?.FoodImageUrls;
                
                // Use model to create food object in DB
                var newFood = await _foodRepository.AddFoodAsync(foodModel, foodImageUrls);

                //Map model to DTO
                var foodDto = _mapper.Map<FoodDTO>(newFood);

                return foodDto;

            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to create food.", exception);
            }
        }
     
        public async Task<FoodDTO> UpdateFoodAsync(Guid foodId, AddUpdateFoodDTO updateFood)
        {
            try
            {
                var loggedInUserId = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");
   
                // Convert AddUpdateCategoryDTO to model
                var foodModal = _mapper.Map<Food>(updateFood);
                var updatedFood = await _foodRepository.UpdateFoodAsync(foodId, loggedInUserId, foodModal) ?? throw new NotFoundException("Food not found.");

                // Convert Food model to FoodDTO
                var updateFoodDto = _mapper.Map<FoodDTO>(updateFood);
                return updateFoodDto;
            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to update food item.", exception);
            }
        }

        public async Task<FoodDTO> DeleteFoodByIdAsync(Guid foodId)
        {
            try
            {
                var loggedInUserId = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");

                var food = await _foodRepository.DeleteFoodByIdAsync(foodId, loggedInUserId) ?? throw new NotFoundException("Food not found.");
                // Map Food model to FoodDTO
                var foodDto = _mapper.Map<FoodDTO>(food);
                return foodDto;
            } catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to delete food.", exception);
            }
        }
    }
}
