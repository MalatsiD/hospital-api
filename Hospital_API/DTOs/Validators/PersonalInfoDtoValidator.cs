using FluentValidation;
using Hospital_API.DTOs.Employee;

namespace Hospital_API.DTOs.Validators
{
    public class PersonalInfoDtoValidator : AbstractValidator<PersonalInfoDto>
    {
        public PersonalInfoDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty()
                .WithMessage("First Name cannot be empty!")
                .MaximumLength(30).WithMessage("First Name cannot be more than 30 characters!");
            RuleFor(x => x.LastName).NotEmpty()
                .WithMessage("Last Name cannot be empty!")
                .MaximumLength(30).WithMessage("Last Name cannot be more than 30 characters!");
            RuleFor(x => x.DateOfBirth)
                .Must(dateOfBirth => ValidateAge(dateOfBirth) >= 18)
                .WithMessage("Employee must be 18 years and older!");
            RuleFor(x => x.Email).NotEmpty()
                .WithMessage("Email cannot be empty!")
                .EmailAddress().WithMessage("A valid email is required!");
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

        protected int ValidateAge(DateTime date)
        {
            int age = 0;
            age = DateTime.Now.Year - date.Year;

            if(DateTime.Now.DayOfYear < date.DayOfYear)
            {
                age = age - 1;
            }

            return age;
        }
    }
}
