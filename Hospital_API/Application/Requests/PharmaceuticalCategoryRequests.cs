using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddPharmaceuticalCategoryRequest : IRequest<ResponseModelView>
    {
        public PharmaceuticalCategoryDto? PharmaceuticalCategoryDto { get; set; }
    }

    public class UpdatePharmaceuticalCategoryRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public PharmaceuticalCategoryDto? PharmaceuticalCategoryDto { get; set; }
    }

    public class CheckPharmaceuticalCategoryExistRequest : IRequest<ResponseModelView>
    {
        public int CategoryId { get; set; }
    }

    public class CheckPharmaceuticalCategoryNameExistRequest : IRequest<ResponseModelView>
    {
        public int VendorId { get; set; }
        public int CategoryId { get; set; }
        public string? Name { get; set; }
    }

    public class GetSinglePharmaceuticalCategoryRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllPharmaceuticalCategoryRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }
}
