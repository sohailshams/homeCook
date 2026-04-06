using System.Text.Json.Serialization;
using HomeCook.Api.Models;

namespace HomeCook.Api.DTOs
{
    public class PostcodeResponseDto
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("result")]
        public PostcodeResult? Result { get; set; }
    }

    public class PostcodeResult
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }
}
