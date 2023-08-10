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
    public class GenderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddGender(GenderDto genderDto)
        {
            var checkGender = CheckGenderNameExist(genderDto.Name!);

            if(!checkGender.Result.IsSuccessful) 
            {
                return StatusCode(checkGender.Result.StatusCode, checkGender.Result);
            }

            var request = new AddGenderRequest();
            request.GenderDto = genderDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateGender(int id, GenderDto genderDto)
        {
            var checkGender = CheckGenderNameExist(genderDto.Name!, id);

            if (!checkGender.Result.IsSuccessful)
            {
                return StatusCode(checkGender.Result.StatusCode, checkGender.Result);
            }

            var request = new UpdateGenderRequest();
            request.Id = id;
            request.GenderDto = genderDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("ChangeGenderStatus/{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateGenderStatus(int id, StatusChangeDto statusChangeDto)
        {
            var request = new UpdateGenderStatusRequest();
            request.Id = id;
            request.StatusChangeDto = statusChangeDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGender(int id)
        {

            var checkGenderInPersonExistRequest = new CheckGenderInPersonExistRequest();
            checkGenderInPersonExistRequest.GenderId = id;
            var checkGenderResult = await _mediator.Send(checkGenderInPersonExistRequest);

            if (!checkGenderResult.IsSuccessful)
            {
                return StatusCode(checkGenderResult.StatusCode, checkGenderResult);
            }

            var request = new DeleteGenderRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleGender(int id)
        {
            var request = new GetSingleGenderRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GenderList/{active}")]
        public async Task<IActionResult> GetAllGenderList(bool active = true)
        {
            var request = new GetAllGenderListRequest();
            request.Active = active;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGenders()
        {
            var request = new GetAllGenderRequest();
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> CheckGenderNameExist(string name, int? genderId = 0)
        {
            var request = new CheckGenderNameExistRequest();
            request.Name = name;
            request.GenderId = genderId ?? 0;

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
