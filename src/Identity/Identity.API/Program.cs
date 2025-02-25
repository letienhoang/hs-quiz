using Identity.API;
using Identity.API.Database;
using Identity.EntityFramework.Shared.Entities.Identity;
using Identity.API.Services;
using IdentityServer8.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using System.Text.Json;
using System.Net.Mime;
using Identity.API.Configuration.Interfaces;
using Identity.API.Configuration;
using Identity.API.Helpers;
using Identity.API.Configuration.Constants;
using Identity.EntityFramework.Shared.DbContexts;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
string migrationAssembly = typeof(ApplicationDbContext).Assembly.GetName().FullName;

builder.Services.RegisterDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, IdentityServerDataProtectionDbContext>(builder.Configuration);
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(connectionString, sqlOptions =>
//     {
//         sqlOptions.MigrationsAssembly(migrationAssembly);
//         sqlOptions.EnableRetryOnFailure(
//             maxRetryCount: 5,
//             maxRetryDelay: TimeSpan.FromSeconds(30),
//             errorNumbersToAdd: null);
//     }));

builder.Services.AddMvcWithLocalization<UserIdentity<string>, string>(builder.Configuration);

// builder.Services.AddIdentity<UserIdentity, IdentityRole>()
//     .AddEntityFrameworkStores<ApplicationDbContext>()
//     .AddDefaultTokenProviders();

builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddAuthenticationServices<AdminIdentityDbContext, UserIdentity, UserIdentityRole>(builder.Configuration);
builder.Services.AddIdentityServer<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, UserIdentity>(builder.Configuration);

builder.Services.AddTransient<IProfileService, ProfileService>();
builder.Services.AddScoped<IRootConfiguration, RootConfiguration>();
builder.Services.AddScoped(typeof(UserResolver<>));

// Create root configuration
var rootConfiguration = new RootConfiguration();
builder.Configuration.GetSection(ConfigurationConsts.AdminConfigurationKey).Bind(rootConfiguration.AdminConfiguration);
builder.Configuration.GetSection(ConfigurationConsts.RegisterConfigurationKey).Bind(rootConfiguration.RegisterConfiguration);

builder.Services.AddSingleton(rootConfiguration);
builder.Services.AddAuthorizationPolicies(rootConfiguration);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Identity.API", Version = "v1" });
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Health checks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddSqlServer(connectionString!, name: "IdentityDB-check", tags: new string[] { "identitydb" });

builder.Services.AddHealthChecksUI(opt => {
    opt.SetEvaluationTimeInSeconds(15);
    opt.MaximumHistoryEntriesPerEndpoint(60);
    opt.SetApiMaxActiveRequests(1);
    opt.AddHealthCheckEndpoint("Identity API", "/hc");
})
.AddInMemoryStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity.API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
// Add custom security headers
app.UseSecurityHeaders(builder.Configuration);
app.UseMvcLocalizationServices();
app.UseIdentityServer();

app.UseRouting();
app.UseStaticFiles();

app.UseAuthorization();
// Migrate and seed database
app.IdentitySeedingDatabase();

// Health checks
app.MapHealthChecks("/hc", new HealthCheckOptions(){
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseHealthChecksUI(config => {
    config.UIPath = "/hc-ui";
    config.ApiPath = "/hc-api";
});
app.MapHealthChecks("/liveness", new HealthCheckOptions{
    Predicate = r => r.Name.Contains("self")
});
app.MapHealthChecks("/hc-details", new HealthCheckOptions {
    ResponseWriter = async (context, report) =>
    {
        var result = JsonSerializer.Serialize(new{
            status = report.Status.ToString(),
            monitors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
        });
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();