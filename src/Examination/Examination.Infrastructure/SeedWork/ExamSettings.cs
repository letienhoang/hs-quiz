using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examination.Infrastructure.SeedWork
{
    public class ExamSettings
    {
        public required string IdentityUrl { get; set; }
        public required DatabaseSettings DatabaseSettings { get; set; }
    }

    public class DatabaseSettings
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
    }
}