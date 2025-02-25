using Identity.API.ViewModels.Consent;

namespace Identity.API.ViewModels.Device
{
    public class DeviceAuthorizationViewModel: ConsentViewModel
    {
        public string UserCode { get; set; }
        public bool ConfirmUserCode { get; set; }
    }
}