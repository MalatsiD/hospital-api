using Newtonsoft.Json;

namespace Hospital_API.ViewModels
{
    public class EmploymentTypeView
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
