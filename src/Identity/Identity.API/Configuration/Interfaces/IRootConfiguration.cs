using Identity.Shared.Configuration.Configuration.Identity;

namespace Identity.API.Configuration.Interfaces
{
    public interface IRootConfiguration
{
    AdminConfiguration AdminConfiguration { get; }

    RegisterConfiguration RegisterConfiguration { get; }
}
}