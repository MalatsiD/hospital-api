using FluentValidation;
using Hospital_API.DTOs.Address;
using Hospital_API.Enums;

namespace Hospital_API.DTOs.Validators
{
    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.AddressDetail).NotEmpty()
                .WithMessage("Address cannot be empty!");
            RuleFor(x => x.ZipCode).NotEqual(0)
                .WithMessage("Zip Code cannot be empty!");
            RuleFor(x => x.CityId).NotEqual(0)
                .WithMessage("City cannot be empty!");
            RuleFor(x => x.AddressTypeId).NotEqual(0)
                .WithMessage("Address Type cannot be empty!");
        }
    }
}
