using System.Security.Claims;

namespace Identity.EntityFramework.Configuration.Configuration.IdentityServer
{
    public class Client : global::IdentityServer8.Models.Client
    {
        public List<Claim> ClientClaims { get; set; } = new List<Claim>();
    }
}
