using Identity.BusinessLogic.Dtos.Configuration;
using Skoruba.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.IdentityResource
{
    public class IdentityResourcePropertyRequestedEvent : AuditEvent
    {
        public IdentityResourcePropertiesDto IdentityResourceProperties { get; set; }

        public IdentityResourcePropertyRequestedEvent(IdentityResourcePropertiesDto identityResourceProperties)
        {
            IdentityResourceProperties = identityResourceProperties;
        }
    }
}
