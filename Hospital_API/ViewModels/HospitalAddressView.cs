using Newtonsoft.Json;

namespace Hospital_API.ViewModels
{
    public class HospitalAddressView : AddressView
    {
        [JsonProperty("hospitalId")]
        public int HospitalId { get; set; }
    }
}
