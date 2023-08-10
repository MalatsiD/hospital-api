using Newtonsoft.Json;

namespace Hospital_API.DTOs.Address
{
    public class UpdateAddressDto
    {
        [JsonProperty("addressDetail")]
        public string? AddressDetail { get; set; } //street Address/PO BOX/ Private Bag

        [JsonProperty("zipCode")]
        public int ZipCode { get; set; }

        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("addressTypeId")]
        public int AddressTypeId { get; set; }
    }
}
