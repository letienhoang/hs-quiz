using Identity.BusinessLogic.Dtos.Configuration;
using Skoruba.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client
{
    public class ClientRequestedEvent : AuditEvent
    {
        public ClientDto ClientDto { get; set; }

        public ClientRequestedEvent(ClientDto clientDto)
        {
            ClientDto = clientDto;
        }
    }
}