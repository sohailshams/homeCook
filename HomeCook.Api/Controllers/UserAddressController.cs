using HomeCook.Api.DTOs;
using HomeCook.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeCook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressController : ControllerBase
    {
        private readonly ILogger<UserAddressController> _logger;
        private readonly IUserAddressService _userAddressService;
        public UserAddressController(
            ILogger<UserAddressController> logger,
            IUserAddressService userAddressService)
        {
            _logger = logger;
            _userAddressService = userAddressService;
        }

        [HttpPut("AddUserAddress")]
        [Authorize]
        public async Task<IActionResult> AddUserAddress([FromBody] AddressRequestDTO addAddressDTO)
        {
            var newAddress = await _userAddressService.AddAddressAsync(addAddressDTO);
            return Ok(newAddress);
        }
    }
}
