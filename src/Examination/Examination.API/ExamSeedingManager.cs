using Examination.Infrastructure;
using Examination.Infrastructure.SeedWork;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Examination.API
{
    public static class ExamSeedingManager
    {
        public static WebApplication ExamSeedingDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var appName = typeof(ExamSeedingManager).Namespace;
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<ExamMongoDbSeeding>>();
                var settings = services.GetRequiredService<IOptions<ExamSettings>>();
                var mongoClient = services.GetRequiredService<IMongoClient>();
                try
                {
                    new ExamMongoDbSeeding().SeedAsync(mongoClient, settings, logger).Wait();
                    logger.LogInformation("Migrations applied successfully for {ApplicationContext}.", appName);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database for {ApplicationContext}.", appName);
                }

                return app;
            }
        }
    }
}