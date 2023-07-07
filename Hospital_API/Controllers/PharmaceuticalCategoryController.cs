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
    public class PharmaceuticalCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PharmaceuticalCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddPharmaeuticalCategory(PharmaceuticalCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checkVendor = CheckVendorExistExist(categoryDto.VendorId);

            if(!checkVendor.Result.IsSuccessful)
            {
                return StatusCode(checkVendor.Result.StatusCode, checkVendor.Result);
            }

            var checkCategory = CheckPharmaceuticalCategoryNameExist(categoryDto.Name!, categoryDto.VendorId);

            if(checkCategory.Result.IsSuccessful)
            {
                return StatusCode(checkCategory.Result.StatusCode, checkCategory.Result);
            }

            var request = new AddPharmaceuticalCategoryRequest();
            request.PharmaceuticalCategoryDto = categoryDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePharmaeuticalCategory(int id, PharmaceuticalCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checkVendor = CheckVendorExistExist(categoryDto.VendorId);

            if (!checkVendor.Result.IsSuccessful)
            {
                return StatusCode(checkVendor.Result.StatusCode, checkVendor.Result);
            }

            var checkCategory = CheckPharmaceuticalCategoryNameExist(categoryDto.Name!, categoryDto.VendorId, id);

            if (checkCategory.Result.IsSuccessful)
            {
                return StatusCode(checkCategory.Result.StatusCode, checkCategory.Result);
            }

            var request = new UpdatePharmaceuticalCategoryRequest();
            request.Id = id;
            request.PharmaceuticalCategoryDto = categoryDto;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSinglePharmaeuticalCategory(int id)
        {
            var request = new GetSinglePharmaceuticalCategoryRequest();
            request.Id = id;
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPharmaeuticalCategories()
        {
            var request = new GetAllPharmaceuticalCategoryRequest();
            var result = await _mediator.Send(request);

            return StatusCode(result.StatusCode, result);
        }

        private async Task<ResponseModelView> CheckVendorExistExist(int vendorId)
        {
            var request = new CheckVendorExistRequest();
            request.VendorId = vendorId;

            var result = await _mediator.Send(request);

            return result;
        }

        private async Task<ResponseModelView> CheckPharmaceuticalCategoryNameExist(string name, int vendorId, int? categoryId = 0)
        {
            var request = new CheckPharmaceuticalCategoryNameExistRequest();
            request.Name = name;
            request.VendorId = vendorId;
            request.CategoryId = categoryId ?? 0;

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
