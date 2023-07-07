using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddRoleRequest : IRequest<ResponseModelView>
    {
        public RoleDto? RoleDto { get; set; }
    }

    public class UpdateRoleRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public RoleDto? RoleDto { get; set; }
    }

    public class CheckRoleExistRequest : IRequest<ResponseModelView>
    {
        public int RoleId { get; set; }
    }

    public class CheckRoleNameExistRequest : IRequest<ResponseModelView>
    {
        public int RoleId { get; set; }
        public string? Name { get; set; }
    }

    public class GetSingleRoleRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllRoleRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }
}
