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
    public class WardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddWard(WardDto wardDto)
        {
            var checkDepartment = CheckDepartmentExist(wardDto.DepartmentId);

            if(!checkDepartment.Result.IsSuccessful)
            {
                return StatusCode(checkDepartment.Result.StatusCode, checkDepartment.Result);
            }

            var checkWard = CheckWardNameExist(wardDto.Name!, wardDto.DepartmentId);

            if (!checkWard.Result.IsSuccessful)
            {
                return StatusCode(checkWard.Result.StatusCode, checkWard.Result);
            }

            var request = new AddWardRequest();
            request.WardDto = wardDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateWard(int id, WardDto wardDto)
        {
            var checkDepartment = CheckDepartmentExist(wardDto.DepartmentId);

            if (!checkDepartment.Result.IsSuccessful)
            {
                return StatusCode(checkDepartment.Result.StatusCode, checkDepartment.Result);
            }

            var checkWard = CheckWardNameExist(wardDto.Name!, wardDto.DepartmentId, id);

            if (!checkWard.Result.IsSuccessful)
            {
                return StatusCode(checkWard.Result.StatusCode, checkWard.Result);
            }

            var request = new UpdateWardRequest();
            request.Id = id;
            request.WardDto = wardDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleWard(int id)
        {
            var request = new GetSingleWardRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWards()
        {
            var request = new GetAllWardRequest();
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> CheckDepartmentExist(int departmentId)
        {
            var request = new CheckDepartmentExistRequest();
            request.DepartmentId = departmentId;

            var result = await _mediator.Send(request);

            return result;
        }

        private async Task<ResponseModelView> CheckWardNameExist(string name, int departmentId, int? wardId = 0)
        {
            var request = new CheckWardNameExistRequest();
            request.Name = name;
            request.DepartmentId = departmentId;
            request.WardId = wardId ?? 0;

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
