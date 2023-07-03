using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class CountryDtoValidator : AbstractValidator<CountryDto>
    {
        public CountryDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Country name cannot be empty");
            RuleFor(x => x.Code).NotEmpty()
                .WithMessage("Country Code cannot be empty");
        }
    }
}
