using FluentValidation;
using Hospital_API.DTOs.Patient;

namespace Hospital_API.DTOs.Validators
{
    public class PatientAdmitDtoValidator : AbstractValidator<PatientAdmitDto>
    {
        public PatientAdmitDtoValidator()
        {
            RuleFor(x => x.WardId).NotEqual(0)
                .WithMessage("Ward cannot be empty!");
        }
    }
}
