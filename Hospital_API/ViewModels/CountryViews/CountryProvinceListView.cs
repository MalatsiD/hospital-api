using Hospital_API.ViewModels.CityViews;
using Hospital_API.ViewModels.ProvinceViews;
using Newtonsoft.Json;

namespace Hospital_API.ViewModels.CountryViews
{
    public class CountryProvinceListView
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("provinces")]
        public ICollection<ProvinceListView>? Provinces { get; set; }
    }
}
