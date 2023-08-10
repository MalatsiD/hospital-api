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
    public class HospitalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HospitalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddHospital(HospitalDto hospitalDto)
        {
            var checkHospital = CheckHospitalExist(hospitalDto.Name!);

            if(!checkHospital.Result.IsSuccessful)
            {
                return StatusCode(checkHospital.Result.StatusCode, checkHospital.Result);
            }

            var hospitalValidatorRequest = new ValidateHospitalRequest();
            hospitalValidatorRequest.CityIdList = hospitalDto.Addresses!.Select(a => a.CityId).Distinct().ToArray();
            hospitalValidatorRequest.AddressTypeIdList = hospitalDto.Addresses!.Select(a => a.AddressTypeId).Distinct().ToArray();

            var checkResult = await _mediator.Send(hospitalValidatorRequest);
            
            if(checkResult.StatusCode == StatusCodes.Status404NotFound)
            {
                return StatusCode(checkResult.StatusCode, checkResult);
            }

            var request = new AddHospitalRequest();
            request.HospitalDto = hospitalDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateHospital(int id, HospitalDto hospitalDto)
        {
            var checkHospital = CheckHospitalExist(hospitalDto.Name!, id);

            if (!checkHospital.Result.IsSuccessful)
            {
                return StatusCode(checkHospital.Result.StatusCode, checkHospital.Result);
            }

            var hospitalValidatorRequest = new ValidateHospitalRequest();
            hospitalValidatorRequest.CityIdList = hospitalDto.Addresses!.Select(a => a.CityId).Distinct().ToArray();
            hospitalValidatorRequest.AddressTypeIdList = hospitalDto.Addresses!.Select(a => a.AddressTypeId).Distinct().ToArray();

            var checkResult = await _mediator.Send(hospitalValidatorRequest);

            if (checkResult.StatusCode == StatusCodes.Status404NotFound)
            {
                return StatusCode(checkResult.StatusCode, checkResult);
            }

            var request = new UpdateHospitalRequest();
            request.Id = id;
            request.HospitalDto = hospitalDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("ChangeHospitalStatus/{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateHospitalStatus(int id, StatusChangeDto statusChangeDto)
        {
            var request = new UpdateHospitalStatusRequest();
            request.Id = id;
            request.StatusChangeDto = statusChangeDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {

            var checkHospitalInDepartmentExistRequest = new CheckHospitalInDepartmentExistRequest();
            checkHospitalInDepartmentExistRequest.HospitalId = id;
            var checkHospitalResult = await _mediator.Send(checkHospitalInDepartmentExistRequest);

            if (!checkHospitalResult.IsSuccessful)
            {
                return StatusCode(checkHospitalResult.StatusCode, checkHospitalResult);
            }

            var request = new DeleteHospitalRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleHospital(int id)
        {
            var request = new GetSingleHospitalRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("HospitalList/{active}")]
        public async Task<IActionResult> GetAllHospitalList(bool active = true)
        {
            var request = new GetAllHospitalListRequest();
            request.Active = active;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHospitals([FromQuery] HospitalFilterDto hospitalFilterDto)
        {
            var request = new GetAllHospitalRequest();
            request.HospitalFilterDto = hospitalFilterDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> CheckHospitalExist(string name, int? hospitalId = 0)
        {
            var request = new CheckHospitalNameExistRequest();
            request.Name = name;
            request.HospitalId = hospitalId ?? 0;

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
