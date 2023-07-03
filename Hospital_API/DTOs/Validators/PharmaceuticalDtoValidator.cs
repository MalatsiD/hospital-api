using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class PharmaceuticalDtoValidator : AbstractValidator<PharmaceuticalDto>
    {
        public PharmaceuticalDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Name cannot be empty!");
            RuleFor(x => x.PharmaceuticalCategoryId).NotEqual(0)
                .WithMessage("Category cannot be empty!");
        }
    }
}
