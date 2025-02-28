using Identity.BusinessLogic.Dtos.Configuration;
using Skoruba.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client
{
    public class ClientsRequestedEvent : AuditEvent
    {
        public ClientsDto ClientsDto { get; set; }

        public ClientsRequestedEvent(ClientsDto clientsDto)
        {
            ClientsDto = clientsDto;
        }
    }
}