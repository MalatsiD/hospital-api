using Newtonsoft.Json;

namespace Hospital_API.DTOs.Filters
{
    public class PaginationDto
    {
        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
    }
}
