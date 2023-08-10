using Newtonsoft.Json;

namespace Hospital_API.ViewModels.CountryViews
{
    public class CountryListView
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }
    }
}
