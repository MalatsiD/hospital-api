using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class DepartmentEmployeeDtoValidator : AbstractValidator<DepartmentEmployeeDto>
    {
        public DepartmentEmployeeDtoValidator()
        {
            RuleFor(x => x.DepartmentId).NotEqual(0)
                .WithMessage("Department cannot be empty!");
            RuleFor(x => x.EmployeeId).NotEqual(0)
                .WithMessage("Employee cannot be empty!");
        }
    }
}
