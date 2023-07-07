﻿using Hospital_API.Application.Requests;
using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddCity(CityDto cityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checkProvince = CheckProvinceExist(cityDto.ProvinceId);

            if(!checkProvince.Result.IsSuccessful)
            {
                return StatusCode(checkProvince.Result.StatusCode, checkProvince.Result);
            }

            var check = CheckCityExist(cityDto.Name!, cityDto.ProvinceId);

            if (!check.Result.IsSuccessful)
            {
                return StatusCode(check.Result.StatusCode, check.Result);
            }

            var request = new AddCityRequest();
            request.CityDto = cityDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCity(int id, CityDto cityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checkProvince = CheckProvinceExist(cityDto.ProvinceId);

            if (!checkProvince.Result.IsSuccessful)
            {
                return StatusCode(checkProvince.Result.StatusCode, checkProvince.Result);
            }

            var check = CheckCityExist(cityDto.Name!, id);

            if (!check.Result.IsSuccessful)
            {
                return StatusCode(check.Result.StatusCode, check.Result);
            }

            var request = new UpdateCityRequest();
            request.Id = id;
            request.CityDto = cityDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleCity(int id)
        {
            var request = new GetSingleCityRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            var request = new GetAllCityRequest();
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> CheckCityExist(string name, int provinceId, int? cityId = 0)
        {
            var request = new CheckCityNameExistRequest();
            request.Name = name;
            request.CityId = cityId ?? 0;

            var result = await _mediator.Send(request);

            return result;
        }

        private async Task<ResponseModelView> CheckProvinceExist(int provinceId)
        {
            var request = new CheckProvinceExistRequest();
            request.ProvinceId = provinceId;

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
