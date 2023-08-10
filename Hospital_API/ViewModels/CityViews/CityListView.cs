using Newtonsoft.Json;

namespace Hospital_API.ViewModels.CityViews
{
    public class CityListView
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}
