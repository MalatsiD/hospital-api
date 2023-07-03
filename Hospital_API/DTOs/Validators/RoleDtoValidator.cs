using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class RoleDtoValidator : AbstractValidator<RoleDto>
    {
        public RoleDtoValidator()
        {
            RuleFor(x =>  x.Name).NotEmpty()
                .WithMessage("Name cannot be empty!");
        }
    }
}
