using System.Collections.Immutable;

namespace HomeCook.Api.Constants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static readonly ImmutableArray<string> AllRoles = [Admin, User];
    }
}
