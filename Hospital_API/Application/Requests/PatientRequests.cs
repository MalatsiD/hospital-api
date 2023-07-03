using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddPatientRequest : IRequest<ResponseModelView>
    {
        public PatientDto? PatientDto { get; set; }
    }

    public class UpdatePatientRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public PatientDto? PatientDto { get; set; }
    }

    public class ValidatePatientRequest : IRequest<ResponseModelView>
    {
        public int[]? CityIdList { get; set; }
        public int[]? AddressTypeIdList { get; set; }
    }

    public class GetSinglePatientRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllPatientRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }
}
