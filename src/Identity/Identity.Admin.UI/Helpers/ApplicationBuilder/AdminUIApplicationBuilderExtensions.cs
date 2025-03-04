using HealthChecks.UI.Client;
using Identity.Admin.UI.Configuration.Constants;
using Identity.Admin.UI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Admin.UI.Helpers.ApplicationBuilder
{
    public static class AdminUIApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the Identity Admin UI to the pipeline of this application. This method must be called 
        /// between UseRouting() and UseEndpoints().
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseIdentityAdminUI(this IApplicationBuilder app)
        {
            app.UseRoutingDependentMiddleware(app.ApplicationServices.GetRequiredService<TestingConfiguration>());

            return app;
        }

        /// <summary>
        /// Maps the Identity Admin UI to the routes of this application.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="patternPrefix"></param>
        public static IEndpointConventionBuilder MapIdentityAdminUI(this IEndpointRouteBuilder endpoint, string patternPrefix = "/")
        {
            return endpoint.MapAreaControllerRoute(CommonConsts.AdminUIArea, CommonConsts.AdminUIArea, patternPrefix + "{controller=Home}/{action=Index}/{id?}");
        }

        /// <summary>
        /// Maps the Identity Admin UI health checks to the routes of this application.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="pattern"></param>
        public static IEndpointConventionBuilder MapIdentityAdminUIHealthChecks(this IEndpointRouteBuilder endpoint, string pattern = "/health", Action<HealthCheckOptions> configureAction = null)
        {
            var options = new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            };

            configureAction?.Invoke(options);

            return endpoint.MapHealthChecks(pattern, options);
        }
    }
}
