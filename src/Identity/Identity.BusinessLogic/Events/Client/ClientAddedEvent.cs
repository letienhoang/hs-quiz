using Identity.BusinessLogic.Dtos.Configuration;
using Skoruba.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client
{
    public class ClientAddedEvent : AuditEvent
    {
        public ClientDto Client { get; set; }

        public ClientAddedEvent(ClientDto client)
        {
            Client = client;
        }
    }
}