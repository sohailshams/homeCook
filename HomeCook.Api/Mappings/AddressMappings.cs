using HomeCook.Api.Dtos;
using HomeCook.Api.Models;
using NetTopologySuite.Geometries;

namespace HomeCook.Api.Mappings
{
    public static class AddressMappings
    {
        public static AddressDto ToDto(this Address address) => new()
        {
            AddressId = address.AddressId,
            AddressLine1 = address.AddressLine1,
            City = address.City,
            Country = address.Country,
            PostCode = address.PostCode,
            IsPrimary = address.IsPrimary
        };

        // userId and location come from the logged-in user and the postcode lookup, never the request
        public static Address ToModel(this AddressRequestDto dto, Guid userId, Point location) => new()
        {
            UserId = userId,
            AddressLine1 = dto.AddressLine1,
            City = dto.City,
            Country = dto.Country,
            PostCode = dto.PostCode,
            IsPrimary = dto.IsPrimary,
            Location = location
        };
    }
}
