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
    public class AilmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AilmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddAilment(AilmentDto ailmentDto)
        {
            var check = CheckAilmentExist(ailmentDto.Name!);

            if(!check.Result.IsSuccessful)
            {
                return StatusCode(check.Result.StatusCode, check.Result);
            }

            var request = new AddAilmentRequest();
            request.AilmentDto = ailmentDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateAilment(int id, AilmentDto ailmentDto)
        {
            var check = CheckAilmentExist(ailmentDto.Name!, id);

            if (!check.Result.IsSuccessful)
            {
                return StatusCode(check.Result.StatusCode, check.Result);
            }

            var request = new UpdateAilmentRequest();
            request.Id = id;
            request.AilmentDto = ailmentDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("ChangeAilmentStatus/{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateAilmentStatus(int id, StatusChangeDto statusChangeDto)
        {
            var request = new UpdateAilmentStatusRequest();
            request.Id = id;
            request.StatusChangeDto = statusChangeDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAilment(int id)
        {

            var checkAilmentInAdmitAilmentExistRequest = new CheckAilmentInAdmitAilmentExistRequest();
            checkAilmentInAdmitAilmentExistRequest.AilmentId = id;
            var checkAilmentResult = await _mediator.Send(checkAilmentInAdmitAilmentExistRequest);

            if (!checkAilmentResult.IsSuccessful)
            {
                return StatusCode(checkAilmentResult.StatusCode, checkAilmentResult);
            }

            var request = new DeleteAilmentRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleAilment(int id)
        {
            var request = new GetSingleAilmentRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("AilmentList/{active}")]
        public async Task<IActionResult> GetAllAilmentList(bool active = true)
        {
            var request = new GetAllAilmentListRequest();
            request.Active = active;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAilments([FromQuery] AilmentFilterDto ailmentFilterDto)
        {
            var request = new GetAllAilmentRequest();
            request.ailmentFilterDto = ailmentFilterDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> CheckAilmentExist(string name, int? ailmentId = 0)
        {
            var requet = new CheckAilmentNameExistRequest();
            requet.Name = name;
            requet.AilmentId = ailmentId ?? 0;

            var result = await _mediator.Send(requet);

            return result;
        }
    }
}
