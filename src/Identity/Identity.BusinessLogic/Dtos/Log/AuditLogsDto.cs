using System.ComponentModel.DataAnnotations;

namespace Identity.BusinessLogic.Dtos.Log
{
    public class AuditLogsDto
    {
        public AuditLogsDto()
        {
            Logs = new List<AuditLogDto>();
        }

        [Required]
        public DateTime? DeleteOlderThan { get; set; }

        public List<AuditLogDto> Logs { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
    }
}