using Hospital_API.Helpers;
using Newtonsoft.Json;

namespace Hospital_API.ViewModels
{
    public class DepartmentView
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
        public bool Active { get; set; }

        [JsonProperty("hospitalId")]
        public int HospitalId { get; set; }

        [JsonProperty("hospitalName")]
        public string? HospitalName { get; set; }
    }
}
