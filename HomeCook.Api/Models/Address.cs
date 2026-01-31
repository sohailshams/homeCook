using System;

namespace HomeCook.Api.Models;

public class Address
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public bool IsPrimary { get; set; }
    public string Line1 { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
    public string Country { get; set; }
}

