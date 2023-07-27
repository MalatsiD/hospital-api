using Newtonsoft.Json;

namespace Hospital_API.DTOs.Filters
{
    public class AddressTypeFilterDto : PaginationDto
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}
