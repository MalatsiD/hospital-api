using Hospital_API.DTOs.Validators;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hospital_API.DTOs
{
    public class VendorDto : IValidatableObject
    {
        [JsonProperty("hospitalId")]
        public int HospitalId { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("phoneNumber")]
        public long PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; } = true;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new VendorDtoValidator();
            var result = validator.Validate(this);

            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] {item.PropertyName}));
        }
    }
}
