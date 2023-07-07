using FluentValidation;
using Hospital_API.DTOs.Patient;

namespace Hospital_API.DTOs.Validators
{
    public class PatientPersonalInfoDtoValidator : AbstractValidator<PatientPersonalInfoDto>
    {
        public PatientPersonalInfoDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty()
               .WithMessage("First Name cannot be empty!")
               .MaximumLength(30).WithMessage("First Name cannot be more than 30 characters!");
            RuleFor(x => x.LastName).NotEmpty()
                .WithMessage("Last Name cannot be empty!")
                .MaximumLength(30).WithMessage("Last Name cannot be more than 30 characters!");
            RuleFor(x => x.PatientNumber).NotEmpty()
                .WithMessage("Patient Number cannot be empty!")
                .MaximumLength(13).WithMessage("Patient Number cannot be more than 13 characters!");
            RuleFor(x => x.DateOfBirth)
                .Must(dateOfBirth => ValidateDateOfBirth(dateOfBirth))
                .WithMessage("Invalid date of birth!");
            RuleFor(x => x.PhoneNumber).NotEmpty()
                .WithMessage("Phone Number cannot be empty!")
                .DependentRules(() =>
                {
                    RuleFor(x => x.PhoneNumber).Length(10)
                    .WithMessage("Phone Number must be 10 digits long!");
                });
            RuleFor(x => x.GenderId).NotEqual(0)
                .WithMessage("Gender cannot be empty!");
            RuleFor(x => x.TitleId).NotEqual(0)
                .WithMessage("Title cannot be empty!");
            RuleFor(x => x.Addresses).Must(addresses => addresses?.Count > 0)
                .WithMessage("Address cannot be empty!");
        }

        protected bool ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if ((DateTime.Now.Year - 120) <= dateOfBirth.Year && (dateOfBirth < DateTime.Now.AddDays(1)))
            {
                return true;
            }

            return false;
        }
    }
}
