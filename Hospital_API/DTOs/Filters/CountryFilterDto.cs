using Newtonsoft.Json;

namespace Hospital_API.DTOs.Filters
{
    public class CountryFilterDto : PaginationDto
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }
    }
}
