using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class PharmaceuticalCategoryDtoValidator : AbstractValidator<PharmaceuticalCategoryDto>
    {
        public PharmaceuticalCategoryDtoValidator()
        {
            RuleFor(x => x.VendorId).NotEqual(0)
                .WithMessage("Vender cannot be empty!");
        }
    }
}
