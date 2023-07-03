using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class TitleDtoValidator : AbstractValidator<TitleDto>
    {
        public TitleDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Title Name cannot be empty!");
        }
    }
}
