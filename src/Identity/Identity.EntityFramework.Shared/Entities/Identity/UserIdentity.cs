using Microsoft.AspNetCore.Identity;

namespace Identity.EntityFramework.Shared.Entities.Identity
{
    public class UserIdentity: IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }

    public class UserIdentity<TKey> : IdentityUser<TKey> where TKey : IEquatable<TKey>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}