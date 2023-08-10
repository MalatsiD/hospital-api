using Hospital_API.ActionFilters;
using Hospital_API.Application.Requests;
using Hospital_API.DTOs;
using Hospital_API.Helpers;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TitleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddTitle(TitleDto titleDto)
        {
            var checkTitle = CheckTitleNameExist(titleDto.Name!);

            if(!checkTitle.Result.IsSuccessful)
            {
                return StatusCode(checkTitle.Result.StatusCode, checkTitle.Result);
            }

            var request = new AddTitleRequest();
            request.TitleDto = titleDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTitle(int id, TitleDto titleDto)
        {
            var checkTitle = CheckTitleNameExist(titleDto.Name!, id);

            if (!checkTitle.Result.IsSuccessful)
            {
                return StatusCode(checkTitle.Result.StatusCode, checkTitle.Result);
            }

            var request = new UpdateTitleRequest();
            request.Id = id;
            request.TitleDto = titleDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("ChangeTitleStatus/{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTitleStatus(int id, StatusChangeDto statusChangeDto)
        {
            var request = new UpdateTitleStatusRequest();
            request.Id = id;
            request.StatusChangeDto = statusChangeDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTitle(int id)
        {

            var checkTitleInPersonExistRequest = new CheckTitleInPersonExistRequest();
            checkTitleInPersonExistRequest.TitleId = id;
            var checkTitleResult = await _mediator.Send(checkTitleInPersonExistRequest);

            if (!checkTitleResult.IsSuccessful)
            {
                return StatusCode(checkTitleResult.StatusCode, checkTitleResult);
            }

            var request = new DeleteTitleRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleTitle(int id)
        {
            var request = new GetSingleTitleRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("TitleList/{active}")]
        public async Task<IActionResult> GetAllTitleList(bool active = true)
        {
            var request = new GetAllTitleListRequest();
            request.Active = active;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTitles()
        {
            var request = new GetAllTitleRequest();
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> CheckTitleNameExist(string name, int? titleId = 0)
        {
            var request = new CheckTitleNameExistRequest();
            request.Name = name;
            request.TitleId = titleId ?? 0;

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
