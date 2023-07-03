using Hospital_API.Application.Requests;
using Hospital_API.DTOs;
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

        [HttpPost]
        public async Task<IActionResult> AddPatient(PatientDto patientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validateRequest = new ValidatePatientRequest();
            validateRequest.CityIdList = patientDto.Addresses!.Select(x => x.CityId).Distinct().ToArray();
            validateRequest.AddressTypeIdList = patientDto.Addresses!.Select(x => x.AddressTypeId).Distinct().ToArray();
            
            var validateResult = await _mediator.Send(validateRequest);

            if(validateResult.StatusCode == StatusCodes.Status404NotFound)
            {
                return StatusCode(validateResult.StatusCode, validateResult);
            }

            var request = new AddPatientRequest();
            request.PatientDto = patientDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, PatientDto patientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validateRequest = new ValidatePatientRequest();
            validateRequest.CityIdList = patientDto.Addresses!.Select(x => x.CityId).Distinct().ToArray();
            validateRequest.AddressTypeIdList = patientDto.Addresses!.Select(x => x.AddressTypeId).Distinct().ToArray();

            var validateResult = await _mediator.Send(validateRequest);

            if (validateResult.StatusCode == StatusCodes.Status404NotFound)
            {
                return StatusCode(validateResult.StatusCode, validateResult);
            }

            var request = new UpdatePatientRequest();
            request.Id = id;
            request.PatientDto = patientDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSinglePatient(int id)
        {
            var request = new GetSinglePatientRequest();
            request.Id = id;
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
    }
}
