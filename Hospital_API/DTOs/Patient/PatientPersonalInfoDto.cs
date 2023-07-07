using Hospital_API.DTOs.Validators;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hospital_API.DTOs.Patient
{
    public class PatientPersonalInfoDto : PersonDto, IValidatableObject
    {
        [JsonProperty("patientNumber")]
        public string? PatientNumber { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new PatientPersonalInfoDtoValidator();
            var result = validator.Validate(this);

            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] {item.PropertyName}));
        }
    }
}
