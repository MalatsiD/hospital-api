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
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
        public async Task<IActionResult> UpdateRole(int id, RoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleRole(int id)
        {
            var request = new GetSingleRoleRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var request = new GetAllRoleRequest();
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
