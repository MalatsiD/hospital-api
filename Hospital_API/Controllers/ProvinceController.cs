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
    public class ProvinceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProvinceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddProvince(ProvinceDto provinceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
        public async Task<IActionResult> UpdateProvince(int id, ProvinceDto provinceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleProvince(int id)
        {
            var request = new GetSingleProvinceRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProvinces()
        {
            var request = new GetAllProvinceRequest();
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
