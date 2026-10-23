using HomeCook.Api.Dtos;
using HomeCook.Api.Models;
using NetTopologySuite.Geometries;

namespace HomeCook.Api.Mappings
{
    public static class ProfileMappings
    {
        // A profile response combines the profile with the user's primary address
        public static UserProfileDto ToDto(this Profile profile, Address address) => new()
        {
            ProfileId = profile.ProfileId,
            UserId = profile.UserId,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            PhoneNumber = profile.PhoneNumber,
            Bio = profile.Bio,
            ProfileImage = profile.ProfileImage,
            AddressLine1 = address.AddressLine1,
            City = address.City,
            Country = address.Country,
            PostCode = address.PostCode,
            IsPrimary = address.IsPrimary
        };

        public static Profile ToProfileModel(this AddUpdateProfileDto dto, Guid userId) => new()
        {
            UserId = userId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Bio = dto.Bio,
            ProfileImage = dto.ProfileImage
        };

        // The profile address is always the user's primary address
        public static Address ToAddressModel(this AddUpdateProfileDto dto, Guid userId, Point location) => new()
        {
            UserId = userId,
            AddressLine1 = dto.AddressLine1,
            City = dto.City,
            Country = dto.Country,
            PostCode = dto.PostCode,
            IsPrimary = true,
            Location = location
        };
    }
}
