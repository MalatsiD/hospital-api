using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class AddressTypeDtoValidator : AbstractValidator<AddressTypeDto>
    {
        public AddressTypeDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Address Type Name cannot be empty!");
        }
    }
}
