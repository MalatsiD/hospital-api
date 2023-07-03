using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class PatientTranferDtoValidator : AbstractValidator<PatientTranferDto>
    {
        public PatientTranferDtoValidator()
        {
            RuleFor(x => x.PatientAdmitId).NotEqual(0)
                .WithMessage("Patient Admitted cannot be empty!");
            RuleFor(x => x.HospitalId).NotEqual(0)
                .WithMessage("Hospital and Ward cannot be empty!")
                .When(x => x.WardId.Equals(0) || x.WardId == null);
        }
    }
}
