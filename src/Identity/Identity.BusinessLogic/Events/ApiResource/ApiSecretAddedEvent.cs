﻿using Skoruba.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.ApiResource
{
    public class ApiSecretAddedEvent : AuditEvent
    {
        public string Type { get; set; }

        public DateTime? Expiration { get; set; }

        public int ApiResourceId { get; set; }

        public ApiSecretAddedEvent(int apiResourceId, string type, DateTime? expiration)
        {
            ApiResourceId = apiResourceId;
            Type = type;
            Expiration = expiration;
        }
    }
}
