using HomeCook.Api.DTOs;
using HomeCook.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeCook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IUserProfileService _userProfileService;
        public ProfileController(ILogger<ProfileController> logger, IUserProfileService userProfileService)
        {
            _logger = logger;
            _userProfileService = userProfileService;
        }

        [HttpGet]
        [Authorize]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserProfile([FromRoute] string userId)
        {
            var userProfile = await _userProfileService.GetUserProfileAsync(userId);

            return Ok(userProfile);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddUserPforile([FromBody] AddUpdateProfileDTO addUserProfile)
        {
            var newUserProfile = await _userProfileService.AddUserProfileAsync(addUserProfile);
            return Ok(newUserProfile);
        }

        [HttpPut]
        [Authorize]
        [Route("update-profile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] AddUpdateProfileDTO updateUserProfile)
        {
           var userProfile = await _userProfileService.UpdateUserProfileAsync(updateUserProfile);
           return Ok(userProfile);
        }
    }
}
