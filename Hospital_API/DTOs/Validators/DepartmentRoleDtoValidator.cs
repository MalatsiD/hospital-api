using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class DepartmentRoleDtoValidator : AbstractValidator<DepartmentRoleDto>
    {
        public DepartmentRoleDtoValidator()
        {
            RuleFor(x => x.DepartmentId).NotEqual(0)
                .WithMessage("Department cannot be empty!");
            RuleFor(x => x.RoleId).NotEqual(0)
                .WithMessage("Role cannot be empty!");
        }
    }
}
