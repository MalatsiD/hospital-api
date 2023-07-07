using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddWardRequest : IRequest<ResponseModelView>
    {
        public WardDto? WardDto { get; set; }
    }

    public class UpdateWardRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public WardDto? WardDto { get; set; }
    }

    public class CheckWardExistRequest : IRequest<ResponseModelView>
    {
        public int WardId { get; set; }
    }

    public class CheckWardNameExistRequest : IRequest<ResponseModelView>
    {
        public int DepartmentId { get; set; }
        public int WardId { get; set; }
        public string? Name { get; set; }
    }

    public class GetSingleWardRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllWardRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }

}
