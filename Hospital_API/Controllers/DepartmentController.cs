using Hospital_API.ActionFilters;
using Hospital_API.Application.Requests;
using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddDepartment(DepartmentDto departmentDto)
        {
            var checkHospital = CheckHospitalExist(departmentDto.HospitalId);

            if(!checkHospital.Result.IsSuccessful)
            {
                return StatusCode(checkHospital.Result.StatusCode, checkHospital.Result);
            }

            var checkDepartment = CheckDepartmentExist(departmentDto.Name!, departmentDto.HospitalId);

            if (!checkDepartment.Result.IsSuccessful)
            {
                return StatusCode(checkHospital.Result.StatusCode, checkHospital.Result);
            }

            var request = new AddDepartmentRequest();
            request.DepartmentDto = departmentDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateDepartment(int id, DepartmentDto departmentDto)
        {
            var checkHospital = CheckHospitalExist(departmentDto.HospitalId);

            if (!checkHospital.Result.IsSuccessful)
            {
                return StatusCode(checkHospital.Result.StatusCode, checkHospital.Result);
            }

            var checkDepartment = CheckDepartmentExist(departmentDto.Name!, departmentDto.HospitalId, id);

            if (!checkDepartment.Result.IsSuccessful)
            {
                return StatusCode(checkHospital.Result.StatusCode, checkHospital.Result);
            }

            var request = new UpdateDepartmentRequest();
            request.Id = id;
            request.DepartmentDto = departmentDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleDepartment(int id)
        {
            var request = new GetSingleDepartmentRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var request = new GetAllDepartmentRequest();
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> CheckDepartmentExist(string name, int hospitalId, int? departmentId = 0)
        {
            var request = new CheckDepartmentNameExistRequest();
            request.Name = name;
            request.HospitalId = hospitalId;
            request.DepartmentId = departmentId ?? 0;

            var result = await _mediator.Send(request);

            return result;
        }

        private async Task<ResponseModelView> CheckHospitalExist(int hospitalId)
        {
            var request = new CheckHospitalExistRequest();
            request.HospitalId = hospitalId;

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
