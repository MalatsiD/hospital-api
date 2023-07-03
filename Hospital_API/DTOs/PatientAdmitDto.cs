using Hospital_API.DTOs.Validators;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hospital_API.DTOs
{
    public class PatientAdmitDto : IValidatableObject
    {
        [JsonProperty("admitDate")]
        public DateTime AdmitDate { get; set; }

        [JsonProperty("admitComment")]
        public string? AdmitComment { get; set; }

        [JsonProperty("dischargeDate")]
        public DateTime DischargeDate { get; set; }

        [JsonProperty("dischargeComment")]
        public string? DischargeComment { get; set; }

        [JsonProperty("prescription")]
        public string? Prescription { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; } = true;

        [JsonProperty("patientId")]
        public int PatientId { get; set; }

        [JsonProperty("wardId")]
        public int WardId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new PatientAdmitDtoValidator();
            var result = validator.Validate(this);

            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] {item.PropertyName}));
        }
    }
}
