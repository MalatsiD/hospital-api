using Hospital_API.DTOs.Patient;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddPatientPersonalInfoRequest : IRequest<ResponseModelView>
    {
        public PatientPersonalInfoDto? PatientPersonalInfo { get; set; }
    }

    public class UpdatePatientPersonalInfoRequest : IRequest<ResponseModelView>
    {
        public int PersonId { get; set; }
        public PatientPersonalInfoDto? PatientPersonalInfo { get; set; }
    }

    public class AddPatientAdmitRequest : IRequest<ResponseModelView>
    {
        public int PatientId { get; set; }
        public PatientAdmitDto? PatientAdmitDto { get; set; }
    }

    public class AddPatientTransferRequest : IRequest<ResponseModelView>
    {
        public int PatientAdmitId { get; set; }
        public PatientTranferDto? PatientTranferDto { get; set; }
    }

    public class UpdatePatientAdmitRequest : IRequest<ResponseModelView>
    {
        public int PatientId { get; set; }
        public int PatientAdmitId { get; set; }
        public PatientAdmitDto? PatientAdmitDto { get; set; }
    }

    public class ValidatePatientPersonalInfoRequest : IRequest<ResponseModelView>
    {
        public int TitleId { get; set; }
        public int GenderId { get; set; }

        public int[]? CityIdList { get; set; }
        public int[]? AddressTypeIdList { get; set; }
    }

    public class CheckPatientExistRequest : IRequest<ResponseModelView>
    {
        public int PatientId { get; set; }
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
