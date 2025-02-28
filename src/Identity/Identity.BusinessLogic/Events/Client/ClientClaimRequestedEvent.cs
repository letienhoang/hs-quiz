using Identity.BusinessLogic.Dtos.Configuration;
using Skoruba.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client
{
    public class ClientClaimRequestedEvent : AuditEvent
    {
        public ClientClaimsDto ClientClaims { get; set; }

        public ClientClaimRequestedEvent(ClientClaimsDto clientClaims)
        {
            ClientClaims = clientClaims;
        }
    }
}