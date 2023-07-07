using FluentValidation;
using Hospital_API.DTOs.Patient;

namespace Hospital_API.DTOs.Validators
{
    public class AddAdmitAilmentDtoValidator : AbstractValidator<AdmitAilmentDto>
    {
        public AddAdmitAilmentDtoValidator()
        {
            RuleFor(x => x.AilmentId).NotEqual(0)
               .WithMessage("Ailment cannot be empty!");
        }
    }
}
