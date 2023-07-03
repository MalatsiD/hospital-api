using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty()
                .WithMessage("Firstname cannot be empty!");
            RuleFor(x => x.LastName).NotEmpty()
                .WithMessage("Lastname cannot be empty!");
            RuleFor(x => x.TitleId).NotEqual(0)
                .WithMessage("Title cannot be empty!");
            RuleFor(x => x.GenderId).NotEqual(0)
                .WithMessage("Gender cannot be empty!");
            RuleFor(x => x.Email).NotEmpty()
                .WithMessage("Email cannot be empty!")
                .EmailAddress().WithMessage("A valid email is required!");
            RuleFor(x => x.HireDate).GreaterThan(DateTime.Now.AddYears(-1))
                .WithMessage("Hire date cannot be more than one year back!")
                .DependentRules(() => {
                    RuleFor(x => x.HireDate).NotNull()
                    .WithMessage("Hire Date cannot be empty!");
                });
        }
    }
}
