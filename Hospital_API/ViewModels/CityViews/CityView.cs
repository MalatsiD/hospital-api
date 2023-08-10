using Newtonsoft.Json;

namespace Hospital_API.ViewModels.CityViews
{
    public class CityView
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("provinceId")]
        public int ProvinceId { get; set; }

        [JsonProperty("provinceName")]
        public string? ProvinceName { get; set; }
    }
}
