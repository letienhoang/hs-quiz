using Identity.API.Database;
using Identity.API.Models;
using Identity.API.Services;
using IdentityServer8.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
string migrationAssembly = typeof(ApplicationDbContext).Assembly.FullName!;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.MigrationsAssembly(migrationAssembly);
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer(x => {
    x.IssuerUri = "https://hsquiz.vn";
    x.Authentication.CookieLifetime = TimeSpan.FromHours(2);
})
    .AddAspNetIdentity<AppUser>()
    .AddDeveloperSigningCredential()
    .AddConfigurationStore(options =>{
        options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, 
            sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(migrationAssembly);
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
    })
    .AddOperationalStore(options =>{
        options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, 
            sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(migrationAssembly);
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
    });
builder.Services.AddTransient<IProfileService, ProfileService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Identity.API", Version = "v1" });
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

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
app.UseIdentityServer();

app.UseRouting();

app.UseAuthorization();

app.Run();