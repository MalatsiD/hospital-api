using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddAddressTypeRequest : IRequest<ResponseModelView>
    {
        public AddressTypeDto? AddressTypeDto { get; set; }
    }

    public class UpdateAddressTypeRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public AddressTypeDto? AddressTypeDto { get; set; }
    }

    public class GetSingleAddressTypeRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllAddressTypeRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }
}
