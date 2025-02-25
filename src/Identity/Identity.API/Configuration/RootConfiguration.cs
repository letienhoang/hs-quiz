using Identity.API.Configuration.Interfaces;
using Identity.Shared.Configuration.Configuration.Identity;

namespace Identity.API.Configuration
{
    public class RootConfiguration: IRootConfiguration
    {      
        public AdminConfiguration AdminConfiguration { get; } = new AdminConfiguration();
        public RegisterConfiguration RegisterConfiguration { get; } = new RegisterConfiguration();
    }
}