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
    public class ProvinceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProvinceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddProvince(ProvinceDto provinceDto)
        {
            var checkCountry = CheckCountryExist(provinceDto.CountryId);

            if(!checkCountry.Result.IsSuccessful)
            {
                return StatusCode(checkCountry.Result.StatusCode, checkCountry.Result);
            }

            var checkProvince = CheckProvinceNameExist(provinceDto.Name!, provinceDto.CountryId);

            if(!checkProvince.Result.IsSuccessful)
            {
                return StatusCode(checkProvince.Result.StatusCode, checkProvince.Result);
            }

            var request = new AddProvinceRequest();
            request.ProvinceDto = provinceDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateProvince(int id, ProvinceDto provinceDto)
        {
            var checkCountry = CheckCountryExist(provinceDto.CountryId);

            if (!checkCountry.Result.IsSuccessful)
            {
                return StatusCode(checkCountry.Result.StatusCode, checkCountry.Result);
            }

            var checkProvince = CheckProvinceNameExist(provinceDto.Name!, provinceDto.CountryId, id);

            if (!checkProvince.Result.IsSuccessful)
            {
                return StatusCode(checkProvince.Result.StatusCode, checkProvince.Result);
            }

            var request = new UpdateProvinceRequest();
            request.Id = id;
            request.ProvinceDto = provinceDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("ChangeProvinceStatus/{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateProvinceStatus(int id, StatusChangeDto statusChangeDto)
        {
            var request = new UpdateProvinceStatusRequest();
            request.Id = id;
            request.StatusChangeDto = statusChangeDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvince(int id)
        {

            var checkProvinceInCityExistRequest = new CheckProvinceInCityExistRequest();
            checkProvinceInCityExistRequest.ProvinceId = id;
            var checkProvinceResult = await _mediator.Send(checkProvinceInCityExistRequest);

            if (!checkProvinceResult.IsSuccessful)
            {
                return StatusCode(checkProvinceResult.StatusCode, checkProvinceResult);
            }

            var request = new DeleteProvinceRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleProvince(int id)
        {
            var request = new GetSingleProvinceRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("ProvinceList/{id}/{active}")]
        public async Task<IActionResult> GetAllProvinceList(int id = 0, bool active = true)
        {
            var request = new GetAllProvinceListRequest();
            request.CountryId = id;
            request.Active = active;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProvinces([FromQuery] ProvinceFilterDto provinceFilterDto)
        {
            var request = new GetAllProvinceRequest();
            request.ProvinceFilterDto = provinceFilterDto;

            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> CheckCountryExist(int countryId)
        {
            var request = new CheckCountryExistRequest();
            request.CountryId = countryId;

            var result = await _mediator.Send(request);

            return result;
        }

        private async Task<ResponseModelView> CheckProvinceNameExist(string name, int countryId, int? provinceId = 0)
        {
            var request = new CheckProvinceNameExistRequest();
            request.Name = name;
            request.CountryId = countryId;
            request.ProvinceId = provinceId ?? 0;

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
