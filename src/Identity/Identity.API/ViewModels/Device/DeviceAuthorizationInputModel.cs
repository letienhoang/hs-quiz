using Identity.API.ViewModels.Consent;

namespace Identity.API.ViewModels.Device
{
    public class DeviceAuthorizationInputModel : ConsentInputModel
    {
        public string UserCode { get; set; }
    }
}