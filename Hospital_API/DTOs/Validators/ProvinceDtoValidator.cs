using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class ProvinceDtoValidator : AbstractValidator<ProvinceDto>
    {
        public ProvinceDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Province Name cannot be empty!");
            RuleFor(x => x.CountryId).NotEqual(0)
                .WithMessage("Country cannot be empty!");
        }
    }
}
