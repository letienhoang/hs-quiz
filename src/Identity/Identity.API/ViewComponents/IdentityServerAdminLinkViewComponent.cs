using Identity.API.Configuration.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.ViewComponents
{
    public class IdentityServerAdminLinkViewComponent: ViewComponent
    {
        private readonly IRootConfiguration _configuration;

        public IdentityServerAdminLinkViewComponent(IRootConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IViewComponentResult Invoke()
        {
            var identityAdminUrl = _configuration.AdminConfiguration.IdentityAdminBaseUrl;

            return View(model: identityAdminUrl);
        }
    }
}