using Newtonsoft.Json;

namespace Hospital_API.DTOs.Address
{
    public class AddressExtendedDto : AddressDto
    {
        [JsonProperty("addressFor")]
        public short? AddressFor { get; set; }

        [JsonProperty("hospitalAddressDto")]
        public virtual HospitalAddressDto? HospitalAddressDto { get; set; }

        [JsonProperty("personAddressDto")]
        public virtual PersonAddressDto? PersonAddressDto { get; set; }
    }
}
