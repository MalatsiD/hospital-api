using Hospital_API.DTOs;
using Hospital_API.DTOs.Filters;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddCountryRequest : IRequest<ResponseModelView>
    {
        public CountryDto? CountryDto { get; set; }
    }

    public class CheckCountryExistRequest : IRequest<ResponseModelView>
    {
        public int CountryId { get; set; }
    }

    public class CheckCountryNameExistRequest : IRequest<ResponseModelView>
    {
        public int CountryId { get; set; }
        public string? Name { get; set; }
    }

    public class UpdateCountryRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public CountryDto? CountryDto { get; set; }
    }

    public class UpdateCountryStatusRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public StatusChangeDto? StatusChangeDto { get; set; }
    }

    public class DeleteCountryRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetSingleCountryRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public bool? Active { get; set; } = true;
    }

    public class GetAllCountryRequest : IRequest<ResponsePaginationModelView>
    {
        public CountryFilterDto? CountryFilterDto { get; set; }
        public bool? Active { get; set; }
    }
}
