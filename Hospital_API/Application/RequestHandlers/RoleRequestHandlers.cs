using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddRoleRequestHandler : IRequestHandler<AddRoleRequest, ResponseModelView>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public AddRoleRequestHandler(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddRoleRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkRoleExist = _repository.FindBy(x => x.Name!.Equals(request.RoleDto!.Name)).FirstOrDefault();

            if (checkRoleExist != null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Role already exist!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            Role role = new Role()
            {
                Name = request.RoleDto!.Name,
                Description= request.RoleDto.Description,
                DateCreated = currentDate,
                DateModified = currentDate,
                Active = request.RoleDto?.Active ?? true
            };

            _repository.Add(role);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Role, RoleView>(role);

            return Task.FromResult(result);
        }
    }

    public class UpdateRoleRequestHandler : IRequestHandler<UpdateRoleRequest, ResponseModelView>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public UpdateRoleRequestHandler(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateRoleRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var role = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (role == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Role not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            role.Name = request.RoleDto!.Name;
            role.Description = request.RoleDto.Description;
            role.DateModified = DateTime.Now;
            role.Active = request.RoleDto?.Active ?? role.Active;

            _repository.Update(role);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Role, RoleView>(role);

            return Task.FromResult(result);
        }
    }

    public class GetSingleRoleRequestHandler : IRequestHandler<GetSingleRoleRequest, ResponseModelView>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public GetSingleRoleRequestHandler(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSingleRoleRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var role = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (role == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Role not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Role, RoleView>(role);

            return Task.FromResult(result);
        }
    }

    public class GetAllRoleRequestHandler : IRequestHandler<GetAllRoleRequest, ResponseModelView>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public GetAllRoleRequestHandler(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllRoleRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var rolesList = _repository.GetAll().ToList();

            if (!rolesList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Roles not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Role>, IEnumerable<RoleView>>(rolesList);

            return Task.FromResult(result);
        }
    }
}
