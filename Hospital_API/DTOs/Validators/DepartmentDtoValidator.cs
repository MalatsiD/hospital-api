using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class DepartmentDtoValidator : AbstractValidator<DepartmentDto>
    {
        public DepartmentDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Department Name cannot be empty!");
            RuleFor(x => x.HospitalId).NotEqual(0)
                .WithMessage("Hospital cannot be empty!");
        }
    }
}
