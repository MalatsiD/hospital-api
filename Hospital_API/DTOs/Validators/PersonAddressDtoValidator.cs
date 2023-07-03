using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class PersonAddressDtoValidator : AbstractValidator<PersonAddressDto>
    {
        public PersonAddressDtoValidator()
        {
            RuleFor(x => x.PersonId).NotEqual(0)
                .WithMessage("Person Details cannot be empty!");
            RuleFor(x => x.AddressId).NotEqual(0)
                .WithMessage("Address cannot be empty!");
        }
    }
}
