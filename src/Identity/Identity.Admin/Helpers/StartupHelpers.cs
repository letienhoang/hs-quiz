using Identity.Admin.UI.Configuration;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Identity.Admin.Helpers
{
    public static class StartupHelpers
    {
        public static void AddAdminUIRazorRuntimeCompilation(this IServiceCollection services, IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                var builder = services.AddControllersWithViews();

                var adminAssembly = typeof(AdminConfiguration).GetTypeInfo().Assembly.GetName().Name;

                builder.AddRazorRuntimeCompilation(options =>
                {
                    if (adminAssembly == null) return;

                    var libraryPath = Path.GetFullPath(Path.Combine(hostingEnvironment.ContentRootPath, "..", adminAssembly));

                    if (Directory.Exists(libraryPath))
                    {
                        options.FileProviders.Add(new PhysicalFileProvider(libraryPath));
                    }
                });
            }
        }
    }
}
