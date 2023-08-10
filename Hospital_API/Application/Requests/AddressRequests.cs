using Hospital_API.DTOs.Address;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddAddressRequest : IRequest<ResponseModelView>
    {
        public AddressExtendedDto? AddressDto { get; set; }
    }

    public class UpdateAddressRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public UpdateAddressDto? UpdateAddressDto { get; set; }
    }

    public class CheckCityInAddressExistRequest : IRequest<ResponseModelView>
    {
        public int CityId { get; set; }
    }

    public class CheckAddressTypeInAddressExistRequest : IRequest<ResponseModelView>
    {
        public int AddressTypeId { get; set; }
    }
}
