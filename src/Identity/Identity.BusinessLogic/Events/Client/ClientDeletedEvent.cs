using Identity.BusinessLogic.Dtos.Configuration;
using Skoruba.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client
{
    public class ClientDeletedEvent : AuditEvent
    {
        public ClientDto Client { get; set; }

        public ClientDeletedEvent(ClientDto client)
        {
            Client = client;
        }
    }
}