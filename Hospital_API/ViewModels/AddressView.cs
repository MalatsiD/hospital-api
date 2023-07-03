using Newtonsoft.Json;

namespace Hospital_API.ViewModels
{
    public class AddressView
    {
        [JsonProperty("addressId")]
        public int AddressId { get; set; }

        [JsonProperty("addressDetail")]
        public string? AddressDetail { get; set; }

        [JsonProperty("zipCode")]
        public int ZipCode { get; set; }

        [JsonProperty("addressTypeId")]
        public int AddressTypeId { get; set; }

        [JsonProperty("addressTypeName")]
        public string? AddressTypeName { get; set; }

        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("cityName")]
        public string? CityName { get; set; }

        [JsonProperty("provinceId")]
        public int ProvinceId { get; set; }

        [JsonProperty("provinceName")]
        public string? ProvinceName { get; set; }

        [JsonProperty("countryId")]
        public int CountryId { get; set; }

        [JsonProperty("countryName")]
        public string? CountryName { get; set; }
    }
}
