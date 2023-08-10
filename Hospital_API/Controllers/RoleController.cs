using Hospital_API.ActionFilters;
using Hospital_API.Application.Requests;
using Hospital_API.DTOs;
using Hospital_API.DTOs.Filters;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddRole(RoleDto roleDto)
        {
            var checkRole = CheckRoleNameExist(roleDto.Name!);

            if(!checkRole.Result.IsSuccessful)
            {
                return StatusCode(checkRole.Result.StatusCode, checkRole.Result);
            }

            var request = new AddRoleRequest();
            request.RoleDto = roleDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateRole(int id, RoleDto roleDto)
        {
            var checkRole = CheckRoleNameExist(roleDto.Name!, id);

            if (!checkRole.Result.IsSuccessful)
            {
                return StatusCode(checkRole.Result.StatusCode, checkRole.Result);
            }

            var request = new UpdateRoleRequest();
            request.Id = id;
            request.RoleDto = roleDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("ChangeRoleStatus/{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateRoleStatus(int id, StatusChangeDto statusChangeDto)
        {
            var request = new UpdateRoleStatusRequest();
            request.Id = id;
            request.StatusChangeDto = statusChangeDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {

            var checkRoleInEmployeeRoleExistRequest = new CheckRoleInEmployeeRoleExistRequest();
            checkRoleInEmployeeRoleExistRequest.RoleId = id;
            var checkRoleResult = await _mediator.Send(checkRoleInEmployeeRoleExistRequest);

            if (!checkRoleResult.IsSuccessful)
            {
                return StatusCode(checkRoleResult.StatusCode, checkRoleResult);
            }

            var request = new DeleteRoleRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleRole(int id)
        {
            var request = new GetSingleRoleRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("RoleList/{active}")]
        public async Task<IActionResult> GetAllRoleList(bool active = true)
        {
            var request = new GetAllRoleListRequest();
            request.Active = active;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles([FromQuery]RoleFilterDto roleFilterDto)
        {
            var request = new GetAllRoleRequest();
            request.RoleFilterDto = roleFilterDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> CheckRoleNameExist(string name, int? roleId = 0)
        {
            var request = new CheckRoleNameExistRequest();
            request.Name = name;
            request.RoleId = roleId ?? 0;

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
