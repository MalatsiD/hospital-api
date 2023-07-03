using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class WardDtoValidator : AbstractValidator<WardDto>
    {
        public WardDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Name cannot be empty!");
            RuleFor(x => x.DepartmentId).NotEqual(0)
                .WithMessage("Department cannot be empty!");
        }
    }
}
