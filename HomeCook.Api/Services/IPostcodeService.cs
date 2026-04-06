using NetTopologySuite.Geometries;

namespace HomeCook.Api.Services
{
    public interface IPostcodeService
    {
        Task<Point> GetLocationAsync(string postcode);
    }
}
