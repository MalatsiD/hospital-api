using Newtonsoft.Json;

namespace Hospital_API.ViewModels.HospitalViews
{
    public class HospitalListView
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("registrationNumber")]
        public string? RegistrationNumber { get; set; }
    }
}
