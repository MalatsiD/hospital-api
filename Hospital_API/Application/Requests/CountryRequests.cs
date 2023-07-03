using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddCountryRequest : IRequest<ResponseModelView>
    {
        public CountryDto? CountryDto { get; set; }
    }

    public class UpdateCountryRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public CountryDto? CountryDto { get; set; }
    }

    public class GetSingleCountryRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public bool? Active { get; set; } = true;
    }

    public class GetAllCountryRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }
}
