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
    public class AddressTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddressTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddAddressType(AddressTypeDto addressTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var check = CheckAddressTypeNameExist(addressTypeDto.Name!);

            if (!check.Result.IsSuccessful)
            {
                return StatusCode(check.Result.StatusCode, check.Result);
            }

            var request = new AddAddressTypeRequest();
            request.AddressTypeDto = addressTypeDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddressType(int id, AddressTypeDto addressTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var check = CheckAddressTypeNameExist(addressTypeDto.Name!, id);

            if (!check.Result.IsSuccessful)
            {
                return StatusCode(check.Result.StatusCode, check.Result);
            }

            var request = new UpdateAddressTypeRequest();
            request.Id = id;
            request.AddressTypeDto = addressTypeDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleAddressType(int id)
        {
            var request = new GetSingleAddressTypeRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAddressTypes()
        {
            var request = new GetAllAddressTypeRequest();
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> CheckAddressTypeNameExist(string name, int? addressTypeId = 0)
        {
            var request = new CheckAddressTypeNameExistRequest();
            request.AddressTypeId = addressTypeId ?? 0;
            request.Name = name;

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
