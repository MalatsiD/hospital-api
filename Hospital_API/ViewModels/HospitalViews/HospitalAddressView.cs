using Newtonsoft.Json;

namespace Hospital_API.ViewModels.HospitalViews
{
    public class HospitalAddressView : AddressView
    {
        [JsonProperty("hospitalId")]
        public int HospitalId { get; set; }
    }
}
