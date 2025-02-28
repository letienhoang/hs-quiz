using Identity.Admin.Api.Configuration;
using Identity.Admin.Api.Helpers;
using Identity.EntityFramework.Shared.DbContexts;
using Microsoft.Extensions.DependencyInjection;
using Skoruba.AuditLogging.EntityFramework.Entities;
using System.Configuration;
using Identity.Shared.Configuration.Helpers;
using Identity.Admin.Api.ExceptionHandling;
using Identity.Admin.Api.Resources;
using Identity.EntityFramework.Shared.Entities.Identity;
using Identity.Admin.Api.Mappers;
using Identity.Shared.Dtos.Identity;
using Identity.Shared.Dtos;
using Identity.BusinessLogic.Identity.Extensions;
using Identity.BusinessLogic.Extensions;
using Identity.Admin.Api.Configuration.Authorization;
using Microsoft.OpenApi.Models;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddJsonFile("serilog.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"serilog.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

builder.Configuration.AddAzureKeyVaultConfiguration(builder.Configuration);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

DockerHelpers.ApplyDockerConfiguration(builder.Configuration);

var adminApiConfiguration = builder.Configuration.GetSection(nameof(AdminApiConfiguration)).Get<AdminApiConfiguration>();

builder.Services.AddSingleton(adminApiConfiguration);
// Add DbContexts
builder.Services.AddDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminLogDbContext, AdminAuditLogDbContext, IdentityServerDataProtectionDbContext, AuditLog>(builder.Configuration);

builder.Services.AddDataProtection<IdentityServerDataProtectionDbContext>(builder.Configuration);

// Add email senders which is currently setup for SendGrid and SMTP
builder.Services.AddEmailSenders(builder.Configuration);

builder.Services.AddScoped<ControllerExceptionFilterAttribute>();
builder.Services.AddScoped<IApiErrorResources, ApiErrorResources>();

// Add authentication services
builder.Services.AddApiAuthentication<AdminIdentityDbContext, UserIdentity, UserIdentityRole>(builder.Configuration);

var profileTypes = new HashSet<Type>
            {
                typeof(IdentityMapperProfile<IdentityRoleDto, IdentityUserRolesDto, string, IdentityUserClaimsDto, IdentityUserClaimDto, IdentityUserProviderDto, IdentityUserProvidersDto, IdentityUserChangePasswordDto, IdentityRoleClaimDto, IdentityRoleClaimsDto>)
            };

builder.Services.AddAdminAspNetIdentityServices<AdminIdentityDbContext, IdentityServerPersistedGrantDbContext,
                IdentityUserDto, IdentityRoleDto, UserIdentity, UserIdentityRole, string, UserIdentityUserClaim, UserIdentityUserRole,
                UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken,
                IdentityUsersDto, IdentityRolesDto, IdentityUserRolesDto,
                IdentityUserClaimsDto, IdentityUserProviderDto, IdentityUserProvidersDto, IdentityUserChangePasswordDto,
                IdentityRoleClaimsDto, IdentityUserClaimDto, IdentityRoleClaimDto>(profileTypes);

builder.Services.AddAdminServices<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminLogDbContext>();

builder.Services.AddAdminApiCors(adminApiConfiguration);

builder.Services.AddMvcServices<IdentityUserDto, IdentityRoleDto,
                UserIdentity, UserIdentityRole, string, UserIdentityUserClaim, UserIdentityUserRole,
                UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken,
                IdentityUsersDto, IdentityRolesDto, IdentityUserRolesDto,
                IdentityUserClaimsDto, IdentityUserProviderDto, IdentityUserProvidersDto, IdentityUserChangePasswordDto,
                IdentityRoleClaimsDto, IdentityUserClaimDto, IdentityRoleClaimDto>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(adminApiConfiguration.ApiVersion, new OpenApiInfo { Title = adminApiConfiguration.ApiName, Version = adminApiConfiguration.ApiVersion });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{adminApiConfiguration.IdentityServerBaseUrl}/connect/authorize"),
                TokenUrl = new Uri($"{adminApiConfiguration.IdentityServerBaseUrl}/connect/token"),
                Scopes = new Dictionary<string, string> {
                                { adminApiConfiguration.OidcApiName, adminApiConfiguration.ApiName }
                            }
            }
        }
    });
    options.OperationFilter<AuthorizeCheckOperationFilter>();
});


builder.Services.AddAuditEventLogging<AdminAuditLogDbContext, AuditLog>(builder.Configuration);

builder.Services.AddIdSHealthChecks<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminIdentityDbContext, AdminLogDbContext, AdminAuditLogDbContext, IdentityServerDataProtectionDbContext>(builder.Configuration, adminApiConfiguration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.AddForwardHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"{adminApiConfiguration.ApiBaseUrl}/swagger/v1/swagger.json", adminApiConfiguration.ApiName);

    c.OAuthClientId(adminApiConfiguration.OidcSwaggerUIClientId);
    c.OAuthAppName(adminApiConfiguration.ApiName);
    c.OAuthUsePkce();
});

app.UseRouting();

app.UseAuthentication();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
