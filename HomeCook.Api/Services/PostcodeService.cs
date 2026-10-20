
using HomeCook.Api.Dtos;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.Net;

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

            using var httpResponse = await _httpClient.GetAsync($"postcodes/{Uri.EscapeDataString(cleaned)}");

            if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                throw new ValidationException($"Invalid postcode: {postcode}.");

            httpResponse.EnsureSuccessStatusCode();

            var response = await httpResponse.Content.ReadFromJsonAsync<PostcodeResponseDto>();

            if (response?.Result == null)
                throw new ValidationException($"Could not find coordinates for postcode {postcode}.");

            return new Point(response.Result.Longitude, response.Result.Latitude)
            {
                SRID = 4326
            };
        }
    }
}
