using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddDepartmentRequest : IRequest<ResponseModelView>
    {
        public DepartmentDto? DepartmentDto { get; set; }
    }

    public class UpdateDepartmentRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public DepartmentDto? DepartmentDto { get; set; }
    }

    public class CheckDepartmentExistRequest : IRequest<ResponseModelView>
    {
        public int DepartmentId { get; set; }
    }

    public class CheckDepartmentNameExistRequest : IRequest<ResponseModelView>
    {
        public int HospitalId { get; set; }
        public int DepartmentId { get; set; }
        public string? Name { get; set; }
    }

    public class GetSingleDepartmentRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllDepartmentRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }
}
