using Hospital_API.Application.Requests;
using Hospital_API.DTOs;
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
        public async Task<IActionResult> AddAilment(AilmentDto ailmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = new AddAilmentRequest();
            request.AilmentDto = ailmentDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAilment(int id, AilmentDto ailmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = new UpdateAilmentRequest();
            request.Id = id;
            request.AilmentDto = ailmentDto;
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

        [HttpGet]
        public async Task<IActionResult> GetAllAilments()
        {
            var request = new GetAllAilmentRequest();
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }
    }
}
