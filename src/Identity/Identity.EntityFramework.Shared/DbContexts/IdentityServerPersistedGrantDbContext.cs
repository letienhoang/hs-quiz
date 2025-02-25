using Identity.EntityFramework.Interfaces;
using IdentityServer8.EntityFramework.DbContexts;
using IdentityServer8.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Identity.EntityFramework.Shared.DbContexts
{
    public class IdentityServerPersistedGrantDbContext : PersistedGrantDbContext<IdentityServerPersistedGrantDbContext>, IAdminPersistedGrantDbContext
    {
        public IdentityServerPersistedGrantDbContext(DbContextOptions<IdentityServerPersistedGrantDbContext> options, OperationalStoreOptions storeOptions)
            : base(options, storeOptions)
        {
        }
    }
}