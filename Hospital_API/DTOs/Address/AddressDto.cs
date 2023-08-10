using Hospital_API.DTOs.Validators;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hospital_API.DTOs.Address
{
    public class AddressDto : IValidatableObject
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("addressDetail")]
        public string? AddressDetail { get; set; } //street Address/PO BOX/ Private Bag

        [JsonProperty("zipCode")]
        public int ZipCode { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; } = true;

        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("addressTypeId")]
        public int AddressTypeId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new AddressDtoValidator();
            var result = validator.Validate(this);

            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
