using Newtonsoft.Json;

namespace Hospital_API.DTOs.Filters
{
    public class HospitalFilterDto : PaginationDto
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("registrationNumber")]
        public string? RegistrationNumber { get; set; }
    }
}
