using Newtonsoft.Json;

namespace Hospital_API.DTOs
{
    public class StatusChangeDto
    {
        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
