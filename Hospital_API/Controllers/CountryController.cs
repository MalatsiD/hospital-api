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
    public class CountryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddCountry(CountryDto countryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var check = CheckCountryExist(countryDto.Name!);

            if (!check.Result.IsSuccessful)
            {
                return StatusCode(check.Result.StatusCode, check.Result);
            }

            var request = new AddCountryRequest();
            request.CountryDto = countryDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCountry(int id, CountryDto countryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var check = CheckCountryExist(countryDto.Name!, id);

            if (!check.Result.IsSuccessful)
            {
                return StatusCode(check.Result.StatusCode, check.Result);
            }

            var request = new UpdateCountryRequest();
            request.Id = id;
            request.CountryDto = countryDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleCountry(int id)
        {
            var request = new GetSingleCountryRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountries()
        {
            var request = new GetAllCountryRequest();
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> CheckCountryExist(string name, int? countryId = 0)
        {
            var request = new CheckCountryNameExistRequest();
            request.Name = name;
            request.CountryId = countryId ?? 0;

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
