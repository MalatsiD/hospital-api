using Newtonsoft.Json;

namespace Hospital_API.ViewModels
{
    public class ProvinceView
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("countryId")]
        public int CountryId { get; set; }

        [JsonProperty("countryName")]
        public string? CountryName { get; set; }
    }
}
