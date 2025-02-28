using Identity.BusinessLogic.Dtos.Configuration;
using Skoruba.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client
{
    public class ClientClonedEvent : AuditEvent
    {
        public ClientCloneDto Client { get; set; }

        public ClientClonedEvent(ClientCloneDto client)
        {
            Client = client;
        }
    }
}