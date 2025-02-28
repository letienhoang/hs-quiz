using Identity.BusinessLogic.Dtos.Configuration;
using Skoruba.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.ApiResource
{
    public class ApiResourceAddedEvent : AuditEvent
    {
        public ApiResourceDto ApiResource { get; set; }

        public ApiResourceAddedEvent(ApiResourceDto apiResource)
        {
            ApiResource = apiResource;
        }
    }
}