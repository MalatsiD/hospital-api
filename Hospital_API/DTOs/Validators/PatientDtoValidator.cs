using FluentValidation;
using Hospital_API.DTOs.Patient;

namespace Hospital_API.DTOs.Validators
{
    public class PatientDtoValidator : AbstractValidator<PatientDto>
    {
        public PatientDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty()
                .WithMessage("Firstname cannot be empty!");
            RuleFor(x => x.LastName).NotEmpty()
                .WithMessage("Lastname cannot be empty!");
            RuleFor(x => x.TitleId).NotEqual(0)
                .WithMessage("Title cannot be empty!");
            RuleFor(x => x.GenderId).NotEqual(0)
                .WithMessage("Gender cannot be empty!");

            RuleFor(x => x.PatientNumber).NotEmpty()
                .WithMessage("Patient Number cannot be empty!");
        }
    }
}
