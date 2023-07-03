using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class CityDtoValidator : AbstractValidator<CityDto>
    {
        public CityDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("City Name cannot be empty!");
            RuleFor(x => x.ProvinceId).NotEqual(0)
                .WithMessage("Province cannot be empty!");
        }
    }
}
