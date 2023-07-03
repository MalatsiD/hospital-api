using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddVendorRequest : IRequest<ResponseModelView>
    {
        public VendorDto? VendorDto { get; set; }
    }

    public class UpdateVendorRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public VendorDto? VendorDto { get; set; }
    }

    public class GetSingleVendorRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllVendorRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }
}
