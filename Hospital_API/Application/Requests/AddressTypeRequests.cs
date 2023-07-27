using Hospital_API.DTOs;
using Hospital_API.DTOs.Filters;
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

    public class UpdateAddressTypeStatusRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public StatusChangeDto? StatusChangeDto { get; set; }
    }

    public class DeleteAddressTypeRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class CheckAddressTypeExistRequest : IRequest<ResponseModelView>
    {
        public int AddressTypeId { get; set; }
    }

    public class CheckAddressTypeNameExistRequest : IRequest<ResponseModelView>
    {
        public int AddressTypeId { get; set; }
        public string? Name { get; set; }
    }

    public class GetSingleAddressTypeRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllAddressTypeRequest : IRequest<ResponsePaginationModelView>
    {
        public AddressTypeFilterDto? AddressTypeFilterDto { get; set; }
        public bool? Active { get; set; }
    }
}
