using Identity.BusinessLogic.Dtos.Configuration;
using Skoruba.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client
{
    public class ClientPropertiesRequestedEvent : AuditEvent
    {
        public ClientPropertiesDto ClientProperties { get; set; }

        public ClientPropertiesRequestedEvent(ClientPropertiesDto clientProperties)
        {
            ClientProperties = clientProperties;
        }
    }
}