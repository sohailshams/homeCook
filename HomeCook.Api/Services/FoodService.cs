using HomeCook.Api.Mappings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HomeCook.Api.Dtos;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace HomeCook.Api.Services
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;
        public readonly ICategoryService _categoryService;
        public readonly IUserRepository _userRepository;
        private readonly ICategoryReposity _categoryReposity;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Cloudinary _cloudinary;
        private readonly IPostcodeService _postcodeService;

        public FoodService(IFoodRepository foodRepository,
                            ICategoryService categoryService,
                            IUserRepository userRepository,
                            ICategoryReposity categoryReposity,
                            IHttpContextAccessor httpContextAccessor,
                            Cloudinary cloudinary,
                            IPostcodeService postcodeService)
        {
            _foodRepository = foodRepository;
            _categoryService = categoryService;
            _userRepository = userRepository;
            _categoryReposity = categoryReposity;
            _httpContextAccessor = httpContextAccessor;
            _cloudinary = cloudinary;
            _postcodeService = postcodeService;
        }

        public async Task<List<FoodDto>> GetFoodListAsync(string postCode, double? radius)
        {
            var location = await _postcodeService.GetLocationAsync(postCode);
            var food = await _foodRepository.GetFoodListAsync(location, radius);

            // map food list<Food> to food list<FoodDto>
            var foodDto = food.Select(f => f.ToDto()).ToList();
            return foodDto;
        }

        public async Task<FoodDetailDto?> GetFoodDetailAsync(Guid id)
        {
            try
            {
                var food = await _foodRepository.GetFoodDetailAsync(id) ?? throw new NotFoundException("Food not found.");
                // map Food model to FoodDetailDto
                var foodDetailDto = food.ToDetailDto();
                return foodDetailDto;

            }
            catch (Exception exception) when (exception is not NotFoundException)
            {
                throw new DatabaseOperationException("An unexpected error occurred while retrieving food details.", exception);
            }
        }


        public async Task<List<FoodDto>> GetFoodByCategoryIdAsync(Guid categoryId)
        {
            var food = await _foodRepository.GetFoodByCategoryIdAsync(categoryId);

            // map food list<Food> to food list<FoodDto>
            if (food.Count > 0)
            {
                var foodDto = food.Select(f => f.ToDto()).ToList();
                return foodDto;
            }
            return [];
        }

        public async Task<FoodDto> AddFoodAsync(CreateFoodDto addFoodDto)
        {
            try
            {
                var loggedInUserId = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");

                if (!Guid.TryParse(loggedInUserId, out Guid parsedLoggedInUserId))
                {
                    throw new UnauthorizedAccessException("Unauthorized user.");
                }

                // Convert addFoodDto to model - the seller is always the logged-in user
                var foodModel = addFoodDto.ToModel(parsedLoggedInUserId);
                ValidateOrderDeadline(foodModel.AvailableDate, foodModel.OrderDeadline);
                if (foodModel.OrderDeadline <= DateTime.UtcNow)
                {
                    throw new ValidationException("Order deadline must be in the future.");
                }

                var category = await _categoryReposity.GetCategoryByIdAsync(foodModel.CategoryId) ?? throw new NotFoundException($"Category with ID {addFoodDto.CategoryId} not found.");
                foodModel.Category = category;

                var seller = await _userRepository.GetUserByIdAsync(foodModel.SellerId) ?? throw new NotFoundException($"Seller with ID {parsedLoggedInUserId} not found.");
                if (seller.IsProfileComplete != true)
                {
                    throw new ValidationException("User Profile must be completed before listing food.");
                }
                foodModel.Seller = seller;

                // Use model to create food object in DB
                var newFood = await _foodRepository.AddFoodAsync(foodModel);
                var imagePublicIds = newFood.FoodImages.Select(x => x.PublicId).ToList();

                // Update draft tag to food
                await MarkTagAsFoodAsync(imagePublicIds);

                // Map model to FoodDto
                var foodDto = newFood.ToDto();

                return foodDto;

            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to create food.", exception);
            }
        }

        public async Task<FoodDto> UpdateFoodAsync(Guid foodId, UpdateFoodDto foodUpdate)
        {
            try
            {
                var loggedInUserId = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");

                var existingFood = await _foodRepository.GetFoodDetailAsync(foodId) ?? throw new NotFoundException("Food not found.");

                if (existingFood.SellerId.ToString() != loggedInUserId) { throw new UnauthorizedAccessException("You are not authorized to update this food item."); }

                existingFood.Name = foodUpdate.Name;
                existingFood.Description = foodUpdate.Description;
                existingFood.Ingredients = foodUpdate.Ingredients.ToList();
                existingFood.Price = foodUpdate.Price;
                existingFood.QuantityAvailable = foodUpdate.QuantityAvailable;
                existingFood.AvailableDate = foodUpdate.AvailableDate!.Value;

                // A deadline already in the past is fine if unchanged, so sellers can still edit closed food
                var orderDeadline = foodUpdate.OrderDeadline!.Value;
                ValidateOrderDeadline(existingFood.AvailableDate, orderDeadline);
                if (orderDeadline != existingFood.OrderDeadline && orderDeadline <= DateTime.UtcNow)
                {
                    throw new ValidationException("Order deadline must be in the future.");
                }
                existingFood.OrderDeadline = orderDeadline;

                existingFood.CategoryId = foodUpdate.CategoryId;

                // No images in the request keeps the existing ones
                if (foodUpdate.FoodImages != null && foodUpdate.FoodImages.Count > 0)
                {
                    existingFood.FoodImages = foodUpdate.FoodImages.Select(image => image.ToModel()).ToList();
                }

                var updatedFood = await _foodRepository.UpdateFoodAsync(existingFood);

                // Convert Food model to FoodDto
                var updatedFoodDto = updatedFood!.ToDto();

                return updatedFoodDto;
            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to update food item.", exception);
            }
        }

        public async Task<FoodDto> DeleteFoodByIdAsync(Guid foodId)
        {
            try
            {
                var loggedInUserId = (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new UnauthorizedAccessException("Unauthorized user.");

                var food = await _foodRepository.DeleteFoodByIdAsync(foodId, loggedInUserId) ?? throw new NotFoundException("Food not found.");
                // Map Food model to FoodDto
                var foodDto = food.ToDto();
                return foodDto;
            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to delete food.", exception);
            }
        }

        private static void ValidateOrderDeadline(DateTime availableDate, DateTime orderDeadline)
        {
            if (orderDeadline > availableDate)
            {
                throw new ValidationException("Order deadline cannot be after the available date.");
            }
        }

        private async Task MarkTagAsFoodAsync(List<string> publicIds)
        {
            if (publicIds.Count == 0)
            {
                throw new InvalidOperationException("publicId is required");
            }

            var tagParams = new TagParams()
            {
                PublicIds = publicIds,
                Tag = "food",
                Command = TagCommand.Replace
            };

            await _cloudinary.TagAsync(tagParams);
        }
    }
}
