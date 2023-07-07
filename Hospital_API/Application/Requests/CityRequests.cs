using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddCityRequest : IRequest<ResponseModelView>
    {
        public CityDto? CityDto { get; set; }
    }

    public class CheckCityExistRequest : IRequest<ResponseModelView>
    {
        public int CityId { get; set; }
    }

    public class CheckCityNameExistRequest : IRequest<ResponseModelView>
    {
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public string? Name { get; set; }
    }

    public class UpdateCityRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public CityDto? CityDto { get; set; }
    }

    public class GetSingleCityRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllCityRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }
}
