using Hospital_API.ActionFilters;
using Hospital_API.Application.Requests;
using Hospital_API.DTOs;
using Hospital_API.DTOs.Employee;
using Hospital_API.ViewModels;
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

        [HttpPost("PersonalInfo")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddPersonalInfo(PersonalInfoDto personalInfoDto)
        {
            var checkEmial = CheckEmployeeEmailExist(personalInfoDto.Email!);

            if(checkEmial.Result.IsSuccessful)
            {
                return StatusCode(checkEmial.Result.StatusCode, checkEmial.Result);
            }

            var validate = ValidateEmployeePersonalInfo(personalInfoDto);

            if (!validate.Result.IsSuccessful)
            {
                return StatusCode(validate.Result.StatusCode, validate.Result);
            }

            var request = new AddEmployeePersonalInfoRequest();
            request.PersonalInfo = personalInfoDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("EmployementInfo/{employeeId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddEmployementInfo(int employeeId, EmploymentInfoDto employmentInfoDto)
        {
            var validate = ValidateEmploymentInfo(employmentInfoDto);

            if (!validate.Result.IsSuccessful)
            {
                return StatusCode(validate.Result.StatusCode, validate.Result);
            }

            var request = new AddEmploymentInfoRequest();
            request.EmployeeId = employeeId;
            request.EmploymentInfoDto = employmentInfoDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("PersonalInfo/{personId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdatePersonalInfo(int personId, PersonalInfoDto personalInfoDto)
        {
            var validate = ValidateEmployeePersonalInfo(personalInfoDto);

            if (!validate.Result.IsSuccessful)
            {
                return StatusCode(validate.Result.StatusCode, validate.Result);
            }

            var request = new UpdateEmployeePersonalInfoRequest();
            request.PersonId = personId;
            request.PersonalInfo = personalInfoDto;
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

        private async Task<ResponseModelView> ValidateEmployeePersonalInfo(PersonalInfoDto personalInfoDto)
        {
            var request = new ValidateEmployeePersonalInfoRequest();
            request.TitleId = personalInfoDto.TitleId;
            request.GenderId = personalInfoDto.GenderId;
            request.AddressTypeIdList = personalInfoDto.Addresses!.Select(x => x.AddressTypeId).Distinct().ToArray();
            request.CityIdList = personalInfoDto.Addresses!.Select(x => x.CityId).Distinct().ToArray();

            var result = await _mediator.Send(request);

            return result;
        }

        private async Task<ResponseModelView> ValidateEmploymentInfo(EmploymentInfoDto employmentInfoDto)
        {
            var request = new ValidateEmploymentInfoRequest();
            request.ManagerId = employmentInfoDto.ManagerId;
            request.DepartmentId = employmentInfoDto.DepartmentId;
            request.RoleId = employmentInfoDto.RoleId;

            var result = await _mediator.Send(request);

            return result;
        }

        private async Task<ResponseModelView> CheckEmployeeEmailExist(string email, int? employeeId = 0)
        {
            var request = new CheckEmployeeEmailExistRequest();
            request.Email = email;
            request.EmployeeId = employeeId ?? 0;

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
