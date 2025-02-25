using Identity.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.EntityFramework.Interfaces
{
    public interface IAdminLogDbContext
    {
        DbSet<Log> Logs { get; set; }
    }
}