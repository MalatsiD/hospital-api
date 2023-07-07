using Hospital_API.DTOs;
using Hospital_API.DTOs.Employee;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddEmployeePersonalInfoRequest : IRequest<ResponseModelView>
    {
        public PersonalInfoDto? PersonalInfo { get; set; }
    }

    public class UpdateEmployeePersonalInfoRequest : IRequest<ResponseModelView>
    {
        public int PersonId { get; set; }
        public PersonalInfoDto? PersonalInfo { get; set; }
    }

    public class AddEmploymentInfoRequest : IRequest<ResponseModelView>
    {
        public int EmployeeId { get; set; }
        public EmploymentInfoDto? EmploymentInfoDto { get; set; }
    }

    public class ValidateEmployeePersonalInfoRequest : IRequest<ResponseModelView>
    {
        public int TitleId { get; set; }
        public int GenderId { get; set; }

        public int[]? CityIdList { get; set; }
        public int[]? AddressTypeIdList { get; set; }
    }

    public class ValidateEmploymentInfoRequest : IRequest<ResponseModelView>
    {
        public int DepartmentId { get; set; }
        public int ManagerId { get; set; }

        public int RoleId { get; set; }
    }

    public class CheckEmployeeExistRequest : IRequest<ResponseModelView>
    {
        public int EmployeeId { get; set; }
    }

    public class CheckEmployeeEmailExistRequest : IRequest<ResponseModelView>
    {
        public int EmployeeId { get; set; }
        public string? Email { get; set; }
    }

    public class GetSingleEmployeeRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllEmployeeRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }
}
