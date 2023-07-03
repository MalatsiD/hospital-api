using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class AdmitAilmentDtoValidator : AbstractValidator<AdmitAilmentDto>
    {
        public AdmitAilmentDtoValidator()
        {
            RuleFor(x => x.PatientAdmitId).NotEqual(0)
                .WithMessage("Patient Admited cannot be empty!");
            RuleFor(x => x.AilmentId).NotEqual(0)
                .WithMessage("Ailment cannot be empty!");
        }
    }
}
