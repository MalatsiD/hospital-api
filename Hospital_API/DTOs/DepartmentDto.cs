using Hospital_API.DTOs.Validators;
using Hospital_API.Helpers;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hospital_API.DTOs
{
    public class DepartmentDto : IValidatableObject
    {
        private string? _name;

        [JsonProperty("name")]
        public string? Name
        {
            get { return _name; }
            set { _name = value!.TrimStringValue(); }
        }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; } = true;

        [JsonProperty("hospitalId")]
        public int HospitalId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new DepartmentDtoValidator();
            var result = validator.Validate(this);

            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
