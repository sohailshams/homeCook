
using HomeCook.Api.DTOs;
using HomeCook.Api.Exceptions;
using NetTopologySuite.Geometries;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HomeCook.Api.Services
{
    public class PostcodeService : IPostcodeService
    {
        private readonly HttpClient _httpClient;

        public PostcodeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Point> GetLocationAsync(string postcode)
        {
            var cleaned = postcode.Replace(" ", "");

            var response = await _httpClient.GetFromJsonAsync<PostcodeResponseDto>($"postcodes/{cleaned}");

            if (response?.Status != 200 || response.Result == null)
                throw new NotFoundException($"Could not find coordinates for postcode {postcode}.");

            return new Point(response.Result.Longitude, response.Result.Latitude)
            {
                SRID = 4326
            };
        }
    }
}
