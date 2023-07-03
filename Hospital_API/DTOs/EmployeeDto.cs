using Hospital_API.DTOs.Validators;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hospital_API.DTOs
{
    public class EmployeeDto : PersonDto, IValidatableObject
    {
        [JsonProperty("hireDate")]
        public DateTime HireDate { get; set; }

        [JsonProperty("terminationDate")]
        public DateTime TerminationDate { get; set; }

        [JsonProperty("ManagerId")]
        public int ManagerId { get; set; }

        [JsonProperty("ManageStartDate")]
        public DateTime ManageStartDate { get; set; }

        [JsonProperty("ManageEndDate")]
        public DateTime ManageEndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new EmployeeDtoValidator();
            var result = validator.Validate(this);

            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] {item.PropertyName}));
        }
    }
}
