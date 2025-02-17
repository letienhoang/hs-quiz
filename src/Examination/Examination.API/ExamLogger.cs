using System.Reflection;
using Serilog;

namespace Examination.API
{
    public static class ExamLogger
    {
        public static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var appName = typeof(ExamLogger).Namespace;
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.WithProperty("ApplicationContext", appName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day, shared: true)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            return logger;
        }

        public static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

            return builder.Build();
        }
    }
}