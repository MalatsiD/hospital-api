using FluentValidation;

namespace Hospital_API.DTOs.Validators
{
    public class HospitalAddressDtoValidator : AbstractValidator<HospitalAddressDto>
    {
        public HospitalAddressDtoValidator()
        {
            RuleFor(x => x.HospitalId).NotEqual(0)
                .WithMessage("Hospital cannot be empty!");
            RuleFor(x => x.AddressId).NotEqual(0)
                .WithMessage("Address cannot be empty!");
        }
    }
}
