using Hospital_API.DTOs.Validators;
using System.ComponentModel.DataAnnotations;

namespace Hospital_API.DTOs.Employee
{
    public class PersonalInfoDto : PersonDto, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new PersonalInfoDtoValidator();
            var result = validator.Validate(this);

            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
