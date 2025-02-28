using Identity.BusinessLogic.Dtos.Configuration;
using Skoruba.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.ApiScope
{
    public class ApiScopeAddedEvent : AuditEvent
    {
        public ApiScopeDto ApiScope { get; set; }

        public ApiScopeAddedEvent(ApiScopeDto apiScope)
        {
            ApiScope = apiScope;
        }
    }
}
