using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.RequestHandlers
{
    public class CheckRoleInEmployeeRoleExistsRequestHandler : IRequestHandler<CheckRoleInEmployeeRoleExistRequest, ResponseModelView>
    {
        private readonly IEmployeeRoleRepository _reposirory;

        public CheckRoleInEmployeeRoleExistsRequestHandler(IEmployeeRoleRepository reposirory)
        {
            _reposirory = reposirory;
        }

        public Task<ResponseModelView> Handle(CheckRoleInEmployeeRoleExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkRole = _reposirory.FindBy(x => x.RoleId == request.RoleId).Any();

            if(checkRole)
            {
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = "Role cannot be deleted!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.IsSuccessful = true;
            result.Response = checkRole;

            return Task.FromResult(result);
        }
    }
}
