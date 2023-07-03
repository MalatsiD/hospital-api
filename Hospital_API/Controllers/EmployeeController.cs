using Hospital_API.Application.Requests;
using Hospital_API.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validateRequest = new ValidateEmployeeRequest();
            validateRequest.CityIdList = employeeDto.Addresses!.Select(x => x.CityId).Distinct().ToArray();
            validateRequest.AddressTypeIdList = employeeDto.Addresses!.Select(x => x.AddressTypeId).Distinct().ToArray();
            var validateResult = await _mediator.Send(validateRequest);

            if(validateResult.StatusCode == StatusCodes.Status404NotFound)
            {
                return StatusCode(validateResult.StatusCode, validateResult);
            }

            var request = new AddEmployeeRequest();
            request.EmployeeDto = employeeDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validateRequest = new ValidateEmployeeRequest();
            validateRequest.CityIdList = employeeDto.Addresses!.Select(x => x.CityId).Distinct().ToArray();
            validateRequest.AddressTypeIdList = employeeDto.Addresses!.Select(x => x.AddressTypeId).Distinct().ToArray();
            var validateResult = await _mediator.Send(validateRequest);

            if (validateResult.StatusCode == StatusCodes.Status404NotFound)
            {
                return StatusCode(validateResult.StatusCode, validateResult);
            }

            var request = new UpdateEmployeeRequest();
            request.Id = id;
            request.EmployeeDto = employeeDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleEmployee(int id)
        {
            var request = new GetSingleEmployeeRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var request = new GetAllEmployeeRequest();
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }
    }
}
