using Microsoft.AspNetCore.Identity;

namespace Identity.EntityFramework.Shared.Entities.Identity
{
    public class UserIdentity: IdentityUser
    {

    }

    public class UserIdentity<TKey> : IdentityUser<TKey> where TKey : IEquatable<TKey>
    {

    }
}