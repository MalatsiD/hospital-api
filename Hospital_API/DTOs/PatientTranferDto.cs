using Hospital_API.DTOs.Validators;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hospital_API.DTOs
{
    public class PatientTranferDto : IValidatableObject
    {
        [JsonProperty("transferDate")]
        public DateTime TransferDate { get; set; }

        [JsonProperty("reason")]
        public string? Reason { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; } = true;

        [JsonProperty("patientAdmitId")]
        public int PatientAdmitId { get; set; }

        [JsonProperty("hospitalId")]
        public int? HospitalId { get; set; } = 0; //Destination Hospital

        [JsonProperty("wardId")]
        public int? WardId { get; set; } = 0;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new PatientTranferDtoValidator();
            var result = validator.Validate(this);

            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName}));
        }
    }
}
