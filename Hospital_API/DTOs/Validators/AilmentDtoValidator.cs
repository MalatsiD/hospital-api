using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class AilmentDtoValidator : AbstractValidator<AilmentDto>
    {
        public AilmentDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Name cannot be empty!");
        }
    }
}
