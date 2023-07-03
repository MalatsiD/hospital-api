using Hospital_API.Application.Requests;
using Hospital_API.DTOs;
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
        public async Task<IActionResult> AddPharmaceutical(PharmaceuticalDto pharmaceuticalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = new AddPharmaceuticalRequest();
            request.PharmaceuticalDto = pharmaceuticalDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePharmaceutical(int id, PharmaceuticalDto pharmaceuticalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
    }
}
