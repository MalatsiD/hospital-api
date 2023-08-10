using Newtonsoft.Json;

namespace Hospital_API.ViewModels.HospitalViews
{
    public class HospitalView
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("registrationNumber")]
        public string? RegistrationNumber { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("addresses")]
        public ICollection<HospitalAddressView>? Addresses { get; set; }
    }
}
