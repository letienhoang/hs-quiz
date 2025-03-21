using Identity.EntityFramework.Shared.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.API.Database
{
    public class ApplicationDbContextSeed
    {
        private readonly IPasswordHasher<UserIdentity> _passwordHasher = new PasswordHasher<UserIdentity>();

        public async Task SeedAsync(ApplicationDbContext context, IWebHostEnvironment env,
            ILogger<ApplicationDbContextSeed> logger, IOptions<AppSettings> settings, int? retry = 0)
        {
            if (retry != null)
            {
                int retryForAvailability = retry.Value;

                try
                {

                    if (!context.Users.Any())
                    {
                        context.Users.AddRange(GetDefaultUser());

                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    if (retryForAvailability < 10)
                    {
                        retryForAvailability++;

                        logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(ApplicationDbContext));

                        await SeedAsync(context, env, logger, settings, retryForAvailability);
                    }
                }
            }
        }

        private IEnumerable<UserIdentity> GetDefaultUser()
        {
            var user =
            new UserIdentity()
            {
                Email = "admin@demo.com",
                Id = Guid.NewGuid().ToString(),
                PhoneNumber = "1234567890",
                UserName = "admin@demo.com",
                NormalizedEmail = "ADMIN@DEMO.COM",
                NormalizedUserName = "ADMIN@DEMO.COM",
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, "Admin@123$");

            return new List<UserIdentity>()
            {
                user
            };
        }
    }
}