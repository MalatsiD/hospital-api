using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class VendorDtoValidator : AbstractValidator<VendorDto>
    {
        public VendorDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Name cannot be empty!");
            RuleFor(x => x.PhoneNumber).NotEqual(0)
                .WithMessage("Name cannot be empty!")
                .DependentRules(() =>
                {
                    RuleFor(x => x.PhoneNumber.ToString()).Length(10)
                    .WithMessage("Phone Number must have 10 digits!");
                });
            RuleFor(x => x.Email).NotEmpty()
                .WithMessage("Email cannot be empty!")
                .EmailAddress().WithMessage("A valid email is required!");

        }
    }
}
