using Hospital_API.DTOs.Validators;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hospital_API.DTOs
{
    public class PatientDto : PersonDto, IValidatableObject
    {
        [JsonProperty("patientNumber")]
        public string? PatientNumber { get; set; } //This should be auto generated

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new PatientDtoValidator();
            var result = validator.Validate(this);

            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] {item.PropertyName}));
        }
    }
}
