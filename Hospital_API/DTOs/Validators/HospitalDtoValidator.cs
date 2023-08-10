using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class HospitalDtoValidator : AbstractValidator<HospitalDto>
    {
        public HospitalDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Hospital Name cannot be empty!");
            RuleFor(x => x.PhoneNumber).NotEmpty()
                .WithMessage("Phone number cannot be empty!")
                .DependentRules(() =>
                {
                    RuleFor(x => x.PhoneNumber).Length(10)
                    .WithMessage("Phone number must be 10 digits long!");
                });
            RuleFor(x => x.Email).NotEmpty()
                .WithMessage("Email cannot be empty!")
                .EmailAddress().WithMessage("A valid email is required!");
            RuleFor(x => x.RegistrationNumber).NotEmpty()
                .WithMessage("Registration Number cannot be empty!");
        }
    }
}
