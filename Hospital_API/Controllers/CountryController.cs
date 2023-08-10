using Hospital_API.ActionFilters;
using Hospital_API.Application.Requests;
using Hospital_API.DTOs;
using Hospital_API.DTOs.Filters;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddCountry(CountryDto countryDto)
        {
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCountry(int id, CountryDto countryDto)
        {
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

        [HttpPut("ChangeCountryStatus/{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCountryStatus(int id, StatusChangeDto statusChangeDto)
        {
            var request = new UpdateCountryStatusRequest();
            request.Id = id;
            request.StatusChangeDto = statusChangeDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {

            var checkCountryInProvinceRequest = new CheckCountryInProvinceExistRequest();
            checkCountryInProvinceRequest.CountryId = id;
            var checkCountryResult = await _mediator.Send(checkCountryInProvinceRequest);

            if (!checkCountryResult.IsSuccessful)
            {
                return StatusCode(checkCountryResult.StatusCode, checkCountryResult);
            }

            var request = new DeleteCountryRequest();
            request.Id = id;
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

        [HttpGet("CountryList/{active}")]
        public async Task<IActionResult> GetAllCountryList(bool active = true)
        {
            var request = new GetAllCountryListRequest();
            request.Active = active;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountries([FromQuery] CountryFilterDto countryFilterDto)
        {
            var request = new GetAllCountryRequest();
            request.CountryFilterDto = countryFilterDto;
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
