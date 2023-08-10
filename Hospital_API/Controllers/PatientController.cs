using Hospital_API.ActionFilters;
using Hospital_API.Application.Requests;
using Hospital_API.DTOs.Employee;
using Hospital_API.DTOs.Patient;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("PatientPersonalInfo")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddPatientPersonalInfo(PatientPersonalInfoDto patientPersonalInfoDto)
        {
            var validate = ValidatePatientPersonalInfo(patientPersonalInfoDto);

            if (!validate.Result.IsSuccessful)
            {
                return StatusCode(validate.Result.StatusCode, validate.Result);
            }

            var request = new AddPatientPersonalInfoRequest();
            request.PatientPersonalInfo = patientPersonalInfoDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("PatientPersonalInfo/{personId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdatePatientPersonalInfo(int personId, PatientPersonalInfoDto patientPersonalInfoDto)
        {
            var validate = ValidatePatientPersonalInfo(patientPersonalInfoDto);

            if (!validate.Result.IsSuccessful)
            {
                return StatusCode(validate.Result.StatusCode, validate.Result);
            }

            var request = new UpdatePatientPersonalInfoRequest();
            request.PersonId = personId;
            request.PatientPersonalInfo = patientPersonalInfoDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("PatientAdmit/{patientId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddPatientAdmit(int patientId, PatientAdmitDto patientAdmitDto)
        {
            var request = new AddPatientAdmitRequest();
            request.PatientId = patientId;
            request.PatientAdmitDto = patientAdmitDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("PatientAdmit/{patientId}/{patientAdmitId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdatePatientAdmit(int patientId, int patientAdmitId, PatientAdmitDto patientAdmitDto)
        {
            var request = new UpdatePatientAdmitRequest();
            request.PatientAdmitId = patientAdmitId;
            request.PatientId = patientId;
            request.PatientAdmitDto = patientAdmitDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("PatientTransfer/{patientAdmitId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddPatientTransfer(int patientAdmitId, PatientTranferDto patientTranferDto)
        {
            var request = new AddPatientTransferRequest();
            request.PatientAdmitId = patientAdmitId;
            request.PatientTranferDto = patientTranferDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetSinglePatient(int patientId)
        {
            var request = new GetSinglePatientRequest();
            request.Id = patientId;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var request = new GetAllPatientRequest();
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> ValidatePatientPersonalInfo(PatientPersonalInfoDto patientPersonalInfoDto)
        {
            var request = new ValidateEmployeePersonalInfoRequest();
            request.TitleId = patientPersonalInfoDto.TitleId;
            request.GenderId = patientPersonalInfoDto.GenderId;
            request.AddressTypeIdList = patientPersonalInfoDto.Addresses!.Select(x => x.AddressTypeId).Distinct().ToArray();
            request.CityIdList = patientPersonalInfoDto.Addresses!.Select(x => x.CityId).Distinct().ToArray();

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
