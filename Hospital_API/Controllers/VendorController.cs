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
    public class VendorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VendorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddVendor(VendorDto vendorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checkHospital = CheckHospitalExist(vendorDto.HospitalId);

            if(!checkHospital.Result.IsSuccessful)
            {
                return StatusCode(checkHospital.Result.StatusCode, checkHospital.Result);
            }

            var checkVendor = CheckVendorNameExist(vendorDto.Name!, vendorDto.HospitalId);

            if(!checkVendor.Result.IsSuccessful)
            {
                return StatusCode(checkVendor.Result.StatusCode, checkVendor.Result);
            }

            var request = new AddVendorRequest();
            request.VendorDto = vendorDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVendor(int id, VendorDto vendorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checkHospital = CheckHospitalExist(vendorDto.HospitalId);

            if (!checkHospital.Result.IsSuccessful)
            {
                return StatusCode(checkHospital.Result.StatusCode, checkHospital.Result);
            }

            var checkVendor = CheckVendorNameExist(vendorDto.Name!, vendorDto.HospitalId, id);

            if (!checkVendor.Result.IsSuccessful)
            {
                return StatusCode(checkVendor.Result.StatusCode, checkVendor.Result);
            }

            var request = new UpdateVendorRequest();
            request.Id = id;
            request.VendorDto = vendorDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleVendor(int id)
        {
            var request = new GetSingleVendorRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVendors()
        {
            var request = new GetAllVendorRequest();
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> CheckHospitalExist(int hospitalId)
        {
            var request = new CheckHospitalExistRequest();
            request.HospitalId = hospitalId;

            var result = await _mediator.Send(request);

            return result;
        }

        private async Task<ResponseModelView> CheckVendorNameExist(string name, int hospitalId, int? vendorId = 0)
        {
            var request = new CheckVendorNameExistRequest();
            request.Name = name;
            request.HospitalId = hospitalId;
            request.VendorId = vendorId ?? 0;

            var result = await _mediator.Send(request);

            return result;
        }
    }
}

