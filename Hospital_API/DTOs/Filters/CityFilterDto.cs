using Newtonsoft.Json;

namespace Hospital_API.DTOs.Filters
{
    public class CityFilterDto : PaginationDto
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("provinceName")]
        public string? ProvinceName { get; set; }
    }
}
