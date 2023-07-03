using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class EmployeeManagerDtoValidator : AbstractValidator<EmployeeManagerDto>
    {
        public EmployeeManagerDtoValidator()
        {
            RuleFor(x => x.EmployeeId).NotEqual(0)
                .WithMessage("Employee cannot be empty!");
            RuleFor(x => x.ManagerId).NotEqual(0)
                .WithMessage("Manager cannot be empty!");
        }
    }
}
