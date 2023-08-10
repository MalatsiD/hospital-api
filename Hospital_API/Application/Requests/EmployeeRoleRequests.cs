using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class CheckRoleInEmployeeRoleExistRequest : IRequest<ResponseModelView>
    {
        public int RoleId { get; set; }
    }
}
