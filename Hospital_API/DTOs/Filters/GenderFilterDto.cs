using Newtonsoft.Json;

namespace Hospital_API.DTOs.Filters
{
    public class GenderFilterDto : PaginationDto
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}
