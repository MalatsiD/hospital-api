using Hospital_API.DTOs;
using Hospital_API.DTOs.Filters;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddHospitalRequest : IRequest<ResponseModelView>
    {
        public HospitalDto? HospitalDto { get; set; }
    }

    public class UpdateHospitalRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public HospitalDto? HospitalDto { get; set; }
    }

    public class UpdateHospitalStatusRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public StatusChangeDto? StatusChangeDto { get; set; }
    }

    public class DeleteHospitalRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class ValidateHospitalRequest : IRequest<ResponseModelView>
    {
        public int[]? CityIdList { get; set; }
        public int[]? AddressTypeIdList { get; set; }
    }

    public class CheckHospitalExistRequest : IRequest<ResponseModelView>
    {
        public int HospitalId { get; set; }
    }

    public class CheckHospitalNameExistRequest : IRequest<ResponseModelView>
    {
        public int HospitalId { get; set; }
        public string? Name { get; set; }
    }

    public class GetSingleHospitalRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllHospitalListRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; } = true;
    }

    public class GetAllHospitalRequest : IRequest<ResponsePaginationModelView>
    {
        public HospitalFilterDto? HospitalFilterDto { get; set; }
        public bool? Active { get; set; }
    }
}
