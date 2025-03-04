using Identity.EntityFramework.Configuration.Configuration;
using SqlMigrationAssembly = Identity.EntityFramework.SqlServer.Helpers.MigrationAssembly;
using MySqlMigrationAssembly = Identity.EntityFramework.MySql.Helpers.MigrationAssembly;
using PostgreSQLMigrationAssembly = Identity.EntityFramework.PostgreSQL.Helpers.MigrationAssembly;
using System.Reflection;

namespace Identity.Admin.Configuration.Database
{
    public static class MigrationAssemblyConfiguration
    {
        public static string GetMigrationAssemblyByProvider(DatabaseProviderConfiguration databaseProvider)
        {
            return databaseProvider.ProviderType switch
            {
                DatabaseProviderType.SqlServer => typeof(SqlMigrationAssembly).GetTypeInfo().Assembly.GetName().Name,
                DatabaseProviderType.PostgreSQL => typeof(PostgreSQLMigrationAssembly).GetTypeInfo()
                    .Assembly.GetName()
                    .Name,
                DatabaseProviderType.MySql => typeof(MySqlMigrationAssembly).GetTypeInfo().Assembly.GetName().Name,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
