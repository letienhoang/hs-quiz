using Identity.API.Database;
using Identity.API.Extensions;
using IdentityServer8.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace Identity.API
{
    public static class MigrationManager
    {
        public static WebApplication MigrationDatabase(this WebApplication app) 
        {
            using (var scope = app.Services.CreateScope())
            {
                string appName = app.GetType().Name;
                var configuration = GetConfiguration();
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<WebApplication>>();
                try
                {
                    logger.LogInformation("Applying migrations for {ApplicationContext}...", appName);

                    services.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                    services.GetRequiredService<ApplicationDbContext>().Database.Migrate();
                    services.GetRequiredService<ConfigurationDbContext>().Database.Migrate();

                    var env = services.GetRequiredService<IWebHostEnvironment>();
                    var settings = services.GetRequiredService<IOptions<AppSettings>>();

                    var appDbContext = services.GetRequiredService<ApplicationDbContext>();
                    var appDbLogger = services.GetRequiredService<ILogger<ApplicationDbContextSeed>>();

                    new ApplicationDbContextSeed().SeedAsync(appDbContext, env, appDbLogger, settings).Wait();

                    var configDbContext = services.GetRequiredService<ConfigurationDbContext>();
                    new ConfigurationDbContextSeed().SeedAsync(configDbContext, app.Configuration).Wait();
                    logger.LogInformation("Migrations applied successfully for {ApplicationContext}.", appName);

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database for {ApplicationContext}.", appName);
                }

                return app;
            }
        }

        private static IHost BuildWebHost(IConfiguration configuration)
        {
            return Host.CreateDefaultBuilder()
            .UseSerilog((context, services, config) => config.ReadFrom.Configuration(context.Configuration))
            .ConfigureWebHostDefaults(webBuilder =>{
                webBuilder.CaptureStartupErrors(false);
                webBuilder.ConfigureAppConfiguration(x => x.AddConfiguration(configuration));
                webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
            }).Build();
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", "Identity.API")
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }   

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets(typeof(ApplicationDbContext).Assembly, optional: true);

            return builder.Build();
        }
    }
}