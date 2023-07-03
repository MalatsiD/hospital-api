using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class PatientAdmitDtoValidator : AbstractValidator<PatientAdmitDto>
    {
        public PatientAdmitDtoValidator()
        {
            RuleFor(x => x.PatientId).NotEqual(0)
                .WithMessage("Patient cannot be empty!");
        }
    }
}
