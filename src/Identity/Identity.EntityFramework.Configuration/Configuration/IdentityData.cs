using Identity.EntityFramework.Configuration.Configuration.Identity;
using System.Data;

namespace Identity.EntityFramework.Configuration.Configuration
{
    public class IdentityData
    {
        public List<Role> Roles { get; set; }
        public List<User> Users { get; set; }
    }
}
