using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class EmployeeRoleDtoValidator : AbstractValidator<EmployeeRoleDto>
    {
        public EmployeeRoleDtoValidator()
        {
            RuleFor(x => x.EmployeeId).NotEqual(0)
                .WithMessage("Employee cannot be empty!");
            RuleFor(x => x.RoleId).NotEqual(0)
                .WithMessage("Role cannot be empty!");
        }
    }
}
