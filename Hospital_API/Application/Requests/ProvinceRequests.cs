using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddProvinceRequest : IRequest<ResponseModelView>
    {
        public ProvinceDto? ProvinceDto { get; set; }
    }

    public class UpdateProvinceRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public ProvinceDto? ProvinceDto { get; set; }
    }

    public class GetSingleProvinceRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllProvinceRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }
}
