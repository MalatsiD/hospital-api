using Newtonsoft.Json;

namespace Hospital_API.ViewModels
{
    public class WardView
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
        public bool Active { get; set; } = true;

        [JsonProperty("departmentId")]
        public int DepartmentId { get; set; }

        [JsonProperty("departmentName")]
        public string? DepartmentName { get; set; }
    }
}
