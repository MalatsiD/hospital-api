using Newtonsoft.Json;

namespace Hospital_API.ViewModels.PatientViews
{
    public class PatientAddressView : AddressView
    {
        [JsonProperty("patientId")]
        public int PatientId { get; set; }
    }
}
