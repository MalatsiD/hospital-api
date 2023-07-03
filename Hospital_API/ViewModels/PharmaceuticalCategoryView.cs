using Newtonsoft.Json;

namespace Hospital_API.ViewModels
{
    public class PharmaceuticalCategoryView
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("vendorId")]
        public int VendorId { get; set; }

        [JsonProperty("venderName")]
        public string? VenderName { get; set; }
    }
}
