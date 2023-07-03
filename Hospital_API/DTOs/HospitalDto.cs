using Hospital_API.DTOs.Validators;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hospital_API.DTOs
{
    public class HospitalDto : IValidatableObject
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("phoneNumber")]
        public long PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("registrationNumber")]
        public string? RegistrationNumber { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; } = true;

        public virtual ICollection<AddressDto>? Addresses { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new HospitalDtoValidator();
            var result = validator.Validate(this);

            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName}));
        }
    }
}
