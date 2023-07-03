using Newtonsoft.Json;

namespace Hospital_API.ViewModels
{
    public class VendorView
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("phoneNumber")]
        public long PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("hospitalId")]
        public int HospitalId { get; set; }

        [JsonProperty("hospitalName")]
        public string? HospitalName { get; set; }
    }
}
