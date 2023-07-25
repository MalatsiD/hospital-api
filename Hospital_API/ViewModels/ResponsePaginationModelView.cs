using Newtonsoft.Json;

namespace Hospital_API.ViewModels
{
    public class ResponsePaginationModelView : ResponseModelView
    {
        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("totalRecords")]
        public int TotalRecords { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }
    }
}
