using Hospital_API.DTOs.Validators;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hospital_API.DTOs.Patient
{
    public class AdmitAilmentDto : IValidatableObject
    {
        [JsonProperty("ailmentId")]
        public int AilmentId { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; } = true;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new AddAdmitAilmentDtoValidator();
            var result = validator.Validate(this);

            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
