using Hospital_API.DTOs;
using Hospital_API.DTOs.Filters;
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

    public class UpdateProvinceStatusRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public StatusChangeDto? StatusChangeDto { get; set; }
    }

    public class DeleteProvinceRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class CheckProvinceExistRequest : IRequest<ResponseModelView>
    {
        public int ProvinceId { get; set; }
    }

    public class CheckProvinceNameExistRequest : IRequest<ResponseModelView>
    {
        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
        public string? Name { get; set; }
    }

    public class CheckCountryInProvinceExistRequest : IRequest<ResponseModelView>
    {
        public int CountryId { get; set; }
    }

    public class GetSingleProvinceRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllProvinceListRequest : IRequest<ResponseModelView>
    {
        public int CountryId { get; set; }
        public bool? Active { get; set; } = true;
    }

    public class GetAllProvinceRequest : IRequest<ResponsePaginationModelView>
    {
        public ProvinceFilterDto? ProvinceFilterDto { get; set; }
        public bool? Active { get; set; }
    }
}
