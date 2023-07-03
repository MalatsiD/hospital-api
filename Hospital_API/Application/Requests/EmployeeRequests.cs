using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddEmployeeRequest : IRequest<ResponseModelView>
    {
        public EmployeeDto? EmployeeDto { get; set; }
    }

    public class UpdateEmployeeRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public EmployeeDto? EmployeeDto { get; set; }
    }

    public class ValidateEmployeeRequest : IRequest<ResponseModelView>
    {
        public int[]? CityIdList { get; set; }
        public int[]? AddressTypeIdList { get; set; }
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
