using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.Helpers;
using Hospital_API.ViewModels;
using Hospital_API.ViewModels.CountryViews;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            result.IsSuccessful = true;
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
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            role.Name = request.RoleDto!.Name;
            role.Description = request.RoleDto.Description;
            role.DateModified = DateTime.Now;
            role.Active = request.RoleDto?.Active ?? role.Active;

            _repository.Update(role);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Role, RoleView>(role);

            return Task.FromResult(result);
        }
    }

    public class UpdateRoleStatusRequestHandler : IRequestHandler<UpdateRoleStatusRequest, ResponseModelView>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public UpdateRoleStatusRequestHandler(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateRoleStatusRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var role = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (role == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Role not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            role.DateModified = DateTime.Now;
            role.Active = request.StatusChangeDto?.Active ?? role.Active;

            _repository.Update(role);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Role, RoleView>(role);

            return Task.FromResult(result);
        }
    }

    public class DeleteRoleRequestHandler : IRequestHandler<DeleteRoleRequest, ResponseModelView>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public DeleteRoleRequestHandler(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(DeleteRoleRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var role = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (role == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Role not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var res = _repository.Delete(role);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = res;

            return Task.FromResult(result);
        }
    }

    public class CheckRoleExistRequestHandler : IRequestHandler<CheckRoleExistRequest, ResponseModelView>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public CheckRoleExistRequestHandler(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckRoleExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var roleExist = _repository.FindBy(x => x.Id == request.RoleId).AsNoTracking().Any();

            if (!roleExist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Role not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = roleExist;

            return Task.FromResult(result);
        }
    }

    public class CheckRoleNameExistRequestHandler : IRequestHandler<CheckRoleNameExistRequest, ResponseModelView>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public CheckRoleNameExistRequestHandler(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckRoleNameExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var role = _repository.FindBy(x => 
                x.Name!.Equals(request.Name)
            );

            if(request.RoleId > 0)
            {
                role = role.Where(x => x.Id != request.RoleId);
            }

            var exist = role.AsNoTracking().Any();

            if (exist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Role already exist!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = exist;

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

            var role = _repository.FindBy(x => x.Id == request.Id).AsNoTracking().FirstOrDefault();

            if (role == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Role not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Role, RoleView>(role);

            return Task.FromResult(result);
        }
    }

    public class GetAllRoleListRequestHandler : IRequestHandler<GetAllRoleListRequest, ResponseModelView>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public GetAllRoleListRequestHandler(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllRoleListRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var roles = _repository.FindBy(x => x.Active == request.Active).AsNoTracking().ToList();

            if (!roles.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Roles not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Role>, IEnumerable<RoleView>>(roles);

            return Task.FromResult(result);
        }
    }

    public class GetAllRoleRequestHandler : IRequestHandler<GetAllRoleRequest, ResponsePaginationModelView>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public GetAllRoleRequestHandler(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponsePaginationModelView> Handle(GetAllRoleRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponsePaginationModelView();

            var predicate = PredicateBuilder.True<Role>();

            List<Role>? roleResult = new List<Role>();

            var roles = _repository.GetAll().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.RoleFilterDto!.Name))
            {
                predicate = predicate.And(x => x.Name!.Contains(request.RoleFilterDto.Name))
                    .Or(x => x.Description!.Contains(request.RoleFilterDto.Name));
            }

            roles = roles.Where(predicate);

            if (request.RoleFilterDto.CurrentPage > 0 && request.RoleFilterDto.PageSize > 0)
            {
                var pagedResult = roles.GetPaged(request.RoleFilterDto.CurrentPage,
                    request.RoleFilterDto.PageSize);

                roleResult = pagedResult.Results as List<Role>;
                result.CurrentPage = pagedResult.CurrentPage;
                result.PageSize = pagedResult.PageSize;
                result.TotalRecords = pagedResult.RowCount;
                result.TotalPages = pagedResult.PageCount;

            }
            else
            {
                roleResult = roles.ToList();
            }

            if (!roleResult!.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Roles not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Role>, IEnumerable<RoleView>>(roleResult!);

            return Task.FromResult(result);
        }
    }
}
