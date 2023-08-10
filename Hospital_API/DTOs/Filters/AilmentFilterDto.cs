using Newtonsoft.Json;

namespace Hospital_API.DTOs.Filters
{
    public class AilmentFilterDto : PaginationDto
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}
