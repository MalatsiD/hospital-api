using Hospital_API.DTOs.Validators;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hospital_API.DTOs
{
    public class PersonAddressDto : IValidatableObject
    {
        [JsonProperty("personId")]
        public int PersonId { get; set; }

        [JsonProperty("addressId")]
        public int AddressId { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; } = true;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new PersonAddressDtoValidator();
            var result = validator.Validate(this);

            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] {item.PropertyName}));
        }
    }
}
