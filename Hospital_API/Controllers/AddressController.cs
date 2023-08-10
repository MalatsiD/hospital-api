using Hospital_API.ActionFilters;
using Hospital_API.Application.Requests;
using Hospital_API.DTOs.Address;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddAddress(AddressExtendedDto addressDto)
        {
            var checkCity = CheckCityExist(addressDto.CityId);

            if (!checkCity.Result.IsSuccessful)
            {
                return StatusCode(checkCity.Result.StatusCode, checkCity.Result);
            }

            var checkAddressType = CheckAddressTypeExist(addressDto.AddressTypeId);

            if (!checkAddressType.Result.IsSuccessful)
            {
                return StatusCode(checkAddressType.Result.StatusCode, checkAddressType.Result);
            }

            var request = new AddAddressRequest();
            request.AddressDto = addressDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateAddress(int id, UpdateAddressDto updateAddressDto)
        {
            var checkCity = CheckCityExist(updateAddressDto.CityId);

            if (!checkCity.Result.IsSuccessful)
            {
                return StatusCode(checkCity.Result.StatusCode, checkCity.Result);
            }

            var checkAddressType = CheckAddressTypeExist(updateAddressDto.AddressTypeId);

            if (!checkAddressType.Result.IsSuccessful)
            {
                return StatusCode(checkAddressType.Result.StatusCode, checkAddressType.Result);
            }

            var request = new UpdateAddressRequest();
            request.Id = id;
            request.UpdateAddressDto = updateAddressDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> CheckCityExist(int cityId)
        {
            var request = new CheckCityExistRequest();
            request.CityId = cityId;

            var result = await _mediator.Send(request);

            return result;
        }

        private async Task<ResponseModelView> CheckAddressTypeExist(int addressTypeId)
        {
            var request = new CheckAddressTypeExistRequest();
            request.AddressTypeId = addressTypeId;

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
