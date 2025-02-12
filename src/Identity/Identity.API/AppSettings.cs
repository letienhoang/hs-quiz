using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API
{
    public class AppSettings
    {
        public required string ExamWebAppClient { get; set; }
        public required string ExamWebAdminClient { get; set; }
        public required string ExamWebApiClient { get; set; }
    }
}