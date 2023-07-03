using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class GenderDtoValidator : AbstractValidator<GenderDto>
    {
        public GenderDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Gender Name cannot be empty!");
        }
    }
}
