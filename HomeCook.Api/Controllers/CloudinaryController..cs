using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeCook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudinaryController : ControllerBase
    {
        private readonly ILogger<CloudinaryController> _logger;
        private readonly Cloudinary _cloudinary;

        public CloudinaryController(ILogger<CloudinaryController> logger, Cloudinary cloudinary)
        {
            _logger = logger;
            _cloudinary = cloudinary;
        }

        [HttpPost("signature")]
        [Authorize]
        public IActionResult GetCloudinarySignature()
        {
            try
            {
                // Create Unix timestamp (seconds)
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var parametersToSign = new SortedDictionary<string, object>
                {
                    { "timestamp", timestamp },
                    { "upload_preset", "homeCook" }
                };

                var signature = _cloudinary.Api.SignParameters(parametersToSign);
                if (string.IsNullOrWhiteSpace(signature))
                {
                    _logger.LogError($"Failed to generate Cloudinary signature at timestamp {timestamp} for user {User.Identity?.Name}.");
                    return StatusCode(500, new { errorMessage = "Failed to generate Cloudinary signature." });
                }

                _logger.LogInformation($"Generated Cloudinary signature at timestamp {timestamp} for user {User.Identity?.Name}.");

                return Ok(new
                {
                    timestamp,
                    signature
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to generate Cloudinary signature.");
                return StatusCode(500, new { errorMessage = "Failed to generate Cloudinary signature." });
            }
        }

        [HttpDelete("{publicId}")]
        [Authorize]
        public async Task<IActionResult> DeleteCloudinaryImage(string publicId)
        {
            try
            {
                _logger.LogInformation($"Public Id = {publicId}");
                if (string.IsNullOrWhiteSpace(publicId)) return BadRequest("Public Id is required.");
                var deletionParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deletionParams);

                if (result.Result == "ok")
                    return Ok();

                return BadRequest(result.Error?.Message ?? "Failed to delete Cloudinary image");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to delete Cloudinary image.");
                return StatusCode(500, new { errorMessage = "Failed to delete Cloudinary image." });
            }
        }
    }
}
