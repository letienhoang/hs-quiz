using Identity.EntityFramework.Shared.DbContexts;
using Identity.EntityFramework.Shared.Entities.Identity;
using Identity.Shared.Dtos.Identity;
using Identity.Shared.Dtos;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Identity.Admin.UI.Helpers.DependencyInjection;
using Identity.Admin.Configuration.Database;
using System.IdentityModel.Tokens.Jwt;
using Identity.Admin.Helpers;
using Identity.Shared.Configuration.Helpers;
using Identity.Admin.UI.Helpers.ApplicationBuilder;
using Identity.EntityFramework.Configuration.Configuration;
using Identity.EntityFramework.Shared.Helpers;
using Serilog;

const string SeedArgs = "/seed";
const string MigrateOnlyArgs = "/migrateonly";

// Get configuration from configuration files
IConfiguration configuration = GetConfiguration(args);

// Initialize Logger with configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    DockerHelpers.ApplyDockerConfiguration(configuration);

    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddConfiguration(configuration);

    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

    // Adds the Identity Admin UI with custom options.
    builder.Services.AddIdentityAdminUI<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext,
    AdminLogDbContext, AdminAuditLogDbContext, AuditLog, IdentityServerDataProtectionDbContext,
        UserIdentity, UserIdentityRole, UserIdentityUserClaim, UserIdentityUserRole,
        UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken, string,
        IdentityUserDto, IdentityRoleDto, IdentityUsersDto, IdentityRolesDto, IdentityUserRolesDto,
        IdentityUserClaimsDto, IdentityUserProviderDto, IdentityUserProvidersDto, IdentityUserChangePasswordDto,
        IdentityRoleClaimsDto, IdentityUserClaimDto, IdentityRoleClaimDto>(ConfigureUIOptions);

    // Monitor changes in Admin UI views
    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddAdminUIRazorRuntimeCompilation(builder.Environment);
    }

    // Add email senders which is currently setup for SendGrid and SMTP
    builder.Services.AddEmailSenders(builder.Configuration);

    // Add controllers
    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseIdentityAdminUI();

    app.MapIdentityAdminUI();
    app.MapIdentityAdminUIHealthChecks();
    app.MapControllers();

    bool migrationComplete = await ApplyDbMigrationsWithDataSeedAsync(args, configuration, app);
    if (args.Any(x => x == MigrateOnlyArgs))
    {
        await app.StopAsync();
        if (!migrationComplete)
        {
            Environment.ExitCode = -1;
        }
        return;
    }

    await app.RunAsync();

    void ConfigureUIOptions(IdentityAdminUIOptions options)
    {
        // Applies configuration from appsettings.
        options.BindConfiguration(builder.Configuration);

        if (builder.Environment.IsDevelopment())
        {
            options.Security.UseDeveloperExceptionPage = true;
        }
        else
        {
            options.Security.UseHsts = true;
        }

        // Set migration assembly for application of db migrations
        var migrationsAssembly = MigrationAssemblyConfiguration.GetMigrationAssemblyByProvider(options.DatabaseProvider);
        options.DatabaseMigrations.SetMigrationsAssemblies(migrationsAssembly);

        // Use production DbContexts and auth services.
        options.Testing.IsStaging = false;
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}


/// <summary>
/// Perform migrations and seed data (if command line parameters specified)
/// </summary>
async Task<bool> ApplyDbMigrationsWithDataSeedAsync(string[] args, IConfiguration configuration, IHost host)
{
    bool applyMigrations = args.Any(x => x == SeedArgs);
    if (applyMigrations)
    {
        args = args.Except(new[] { SeedArgs }).ToArray();
    }

    var seedConfiguration = configuration.GetSection(nameof(SeedConfiguration)).Get<SeedConfiguration>();
    var databaseMigrationsConfiguration = configuration.GetSection(nameof(DatabaseMigrationsConfiguration))
        .Get<DatabaseMigrationsConfiguration>();

    return await DbMigrationHelpers.ApplyDbMigrationsWithDataSeedAsync<IdentityServerConfigurationDbContext, AdminIdentityDbContext,
        IdentityServerPersistedGrantDbContext, AdminLogDbContext, AdminAuditLogDbContext,
        IdentityServerDataProtectionDbContext, UserIdentity, UserIdentityRole>(host,
        applyMigrations, seedConfiguration, databaseMigrationsConfiguration);
}

/// <summary>
/// Build application configuration from multiple JSON files and environments.
/// </summary>
IConfiguration GetConfiguration(string[] args)
{
    string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    bool isDevelopment = environment == Environments.Development;

    var configurationBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
        .AddJsonFile("serilog.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"serilog.{environment}.json", optional: true, reloadOnChange: true)
        .AddJsonFile("identitydata.json", optional: true, reloadOnChange: true)
        .AddJsonFile("identityserverdata.json", optional: true, reloadOnChange: true);

    if (isDevelopment)
    {
        configurationBuilder.AddUserSecrets<Program>(true);
    }

    IConfiguration config = configurationBuilder.Build();
    config.AddAzureKeyVaultConfiguration(configurationBuilder);
    configurationBuilder.AddCommandLine(args);
    configurationBuilder.AddEnvironmentVariables();

    return configurationBuilder.Build();
}