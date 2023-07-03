using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class EmploymentTypeDtoValidator : AbstractValidator<EmploymentTypeDto>
    {
        public EmploymentTypeDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Name cannot be empty!");
        }
    }
}
