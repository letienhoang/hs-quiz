﻿using AutoMapper;
using Identity.BusinessLogic.Dtos.Log;
using Identity.EntityFramework.Entities;
using Identity.EntityFramework.Extensions.Common;
using Skoruba.AuditLogging.EntityFramework.Entities;

namespace Identity.BusinessLogic.Mappers
{
    public static class LogMappers
    {
        internal static IMapper Mapper { get; }

        static LogMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<LogMapperProfile>())
                .CreateMapper();
        }

        public static LogDto ToModel(this Log log)
        {
            return Mapper.Map<LogDto>(log);
        }

        public static LogsDto ToModel(this PagedList<Log> logs)
        {
            return Mapper.Map<LogsDto>(logs);
        }

        public static AuditLogsDto ToModel<TAuditLog>(this PagedList<TAuditLog> auditLogs)
            where TAuditLog : AuditLog
        {
            return Mapper.Map<AuditLogsDto>(auditLogs);
        }

        public static AuditLogDto ToModel(this AuditLog auditLog)
        {
            return Mapper.Map<AuditLogDto>(auditLog);
        }

        public static Log ToEntity(this LogDto log)
        {
            return Mapper.Map<Log>(log);
        }
    }
}
