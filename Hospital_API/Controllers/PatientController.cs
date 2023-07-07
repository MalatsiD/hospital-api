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
        public async Task<IActionResult> AddPatientPersonalInfo(PatientPersonalInfoDto patientPersonalInfoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
        public async Task<IActionResult> UpdatePatientPersonalInfo(int personId, PatientPersonalInfoDto patientPersonalInfoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
        public async Task<IActionResult> AddPatientAdmit(int patientId, PatientAdmitDto patientAdmitDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = new AddPatientAdmitRequest();
            request.PatientId = patientId;
            request.PatientAdmitDto = patientAdmitDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("PatientAdmit/{patientId}/{patientAdmitId}")]
        public async Task<IActionResult> UpdatePatientAdmit(int patientId, int patientAdmitId, PatientAdmitDto patientAdmitDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = new UpdatePatientAdmitRequest();
            request.PatientAdmitId = patientAdmitId;
            request.PatientId = patientId;
            request.PatientAdmitDto = patientAdmitDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("PatientTransfer/{patientAdmitId}")]
        public async Task<IActionResult> AddPatientTransfer(int patientAdmitId, PatientTranferDto patientTranferDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
