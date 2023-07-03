using Newtonsoft.Json;

namespace Hospital_API.ViewModels
{
    public class PharmaceuticalView
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("pharmaceuticalCategoryId")]
        public int PharmaceuticalCategoryId { get; set; }

        [JsonProperty("pharmaceuticalCategoryName")]
        public string? PharmaceuticalCategoryName { get; set; }
    }
}
