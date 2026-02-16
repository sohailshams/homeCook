using HomeCook.Api.Models;

namespace HomeCook.Api.Projections;

public class ProfileWithAddress
{
    public ProfileWithAddress(Profile profile, Address address)
    {
        Profile = profile;
        Address = address;
    }
    public Profile Profile { get; set; }
    public Address Address { get; set; }
}
