using Newtonsoft.Json;

namespace Hospital_API.DTOs.Filters
{
    public class ProvinceFilterDto : PaginationDto
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("countryName")]
        public string? CountryName { get; set; }
    }
}
