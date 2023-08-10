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
    public class PharmaceuticalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PharmaceuticalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddPharmaceutical(PharmaceuticalDto pharmaceuticalDto)
        {
            var checkCategory = CheckPharmaceuticalCategoryExist(pharmaceuticalDto.PharmaceuticalCategoryId);

            if(!checkCategory.Result.IsSuccessful)
            {
                return StatusCode(checkCategory.Result.StatusCode, checkCategory.Result);
            }

            var checkPharmaceutical = CheckPharmaceuticalNameExist(pharmaceuticalDto.Name!, pharmaceuticalDto.PharmaceuticalCategoryId);

            if(!checkPharmaceutical.Result.IsSuccessful)
            {
                return StatusCode(checkPharmaceutical.Result.StatusCode, checkPharmaceutical.Result);
            }

            var request = new AddPharmaceuticalRequest();
            request.PharmaceuticalDto = pharmaceuticalDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdatePharmaceutical(int id, PharmaceuticalDto pharmaceuticalDto)
        {
            var checkCategory = CheckPharmaceuticalCategoryExist(pharmaceuticalDto.PharmaceuticalCategoryId);

            if (!checkCategory.Result.IsSuccessful)
            {
                return StatusCode(checkCategory.Result.StatusCode, checkCategory.Result);
            }

            var checkPharmaceutical = CheckPharmaceuticalNameExist(pharmaceuticalDto.Name!, pharmaceuticalDto.PharmaceuticalCategoryId, id);

            if (!checkPharmaceutical.Result.IsSuccessful)
            {
                return StatusCode(checkPharmaceutical.Result.StatusCode, checkPharmaceutical.Result);
            }

            var request = new UpdatePharmaceuticalRequest();
            request.Id = id;
            request.PharmaceuticalDto = pharmaceuticalDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSinglePharmaceutical(int id)
        {
            var request = new GetSinglePharmaceuticalRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPharmaceuticals()
        {
            var request = new GetAllPharmaceuticalRequest();
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> CheckPharmaceuticalCategoryExist(int categoryId)
        {
            var request = new CheckPharmaceuticalCategoryExistRequest();
            request.CategoryId = categoryId;

            var result = await _mediator.Send(request);

            return result;
        }

        private async Task<ResponseModelView> CheckPharmaceuticalNameExist(string name, int categoryId, int? pharmaceuticalId = 0)
        {
            var request = new CheckPharmaceuticalNameExistRequest();
            request.Name = name;
            request.CategoryId = categoryId;
            request.PharmaceuticalId = pharmaceuticalId ?? 0;

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
