using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddWardRequestHandler : IRequestHandler<AddWardRequest, ResponseModelView>
    {
        private readonly IWardRepository _repository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public AddWardRequestHandler(IWardRepository repository, IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _repository = repository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddWardRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkDepartmentExist = _departmentRepository.FindBy(x => x.Id == request.WardDto!.DepartmentId).FirstOrDefault();

            if (checkDepartmentExist == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Department not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var checkWardExist = _repository.FindBy(x => 
                x.Name!.Equals(request.WardDto!.Name) &&
                x.DepartmentId == request.WardDto!.DepartmentId
            ).FirstOrDefault();

            if(checkWardExist != null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Ward already exist!";
                result.Response = false;

                return Task.FromResult(result);
            }
             var currentDate = DateTime.Now;

            Ward ward = new Ward()
            {
                Name = request.WardDto!.Name,
                Code = request.WardDto!.Code,
                Description = request.WardDto!.Description,
                DateCreated = currentDate,
                DateModified = currentDate,
                Active = request.WardDto?.Active ?? true,
                DepartmentId = request.WardDto!.DepartmentId
            };

            _repository.Add(ward);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Ward, WardView>(ward);

            return Task.FromResult(result);
        }
    }

    public class UpdateWardRequestHandler : IRequestHandler<UpdateWardRequest, ResponseModelView>
    {
        private readonly IWardRepository _repository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public UpdateWardRequestHandler(IWardRepository repository, IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _repository = repository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateWardRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkDepartmentExist = _departmentRepository.FindBy(x => x.Id == request.WardDto!.DepartmentId).FirstOrDefault();

            if (checkDepartmentExist == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Department not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var ward = _repository.FindBy(x =>  x.Id == request.Id).FirstOrDefault();

            if (ward == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Ward not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            ward.Name = request.WardDto!.Name;
            ward.Code = request.WardDto!.Code;
            ward.Description = request.WardDto!.Description;
            ward.DateModified = DateTime.Now;
            ward.Active = request.WardDto?.Active ?? ward.Active;
            ward.DepartmentId = request.WardDto!.DepartmentId;

            _repository.Update(ward);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Ward, WardView>(ward);

            return Task.FromResult(result);
        }
    }

    public class GetSingleWardRequestHandler : IRequestHandler<GetSingleWardRequest, ResponseModelView>
    {
        private readonly IWardRepository _repository;
        private readonly IMapper _mapper;

        public GetSingleWardRequestHandler(IWardRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSingleWardRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var ward = _repository.FindBy(x => x.Id == request.Id)
                 .Include(x => x.Department)
                .FirstOrDefault();

            if (ward == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Ward not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Ward, WardView>(ward);

            return Task.FromResult(result);
        }
    }

    public class GetAllWardRequestHandler : IRequestHandler<GetAllWardRequest, ResponseModelView>
    {
        private readonly IWardRepository _repository;
        private readonly IMapper _mapper;

        public GetAllWardRequestHandler(IWardRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllWardRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var wardsList = _repository.GetAll()
                .Include(x => x.Department)
                .ToList();

            if (!wardsList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Wards not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Ward>, IEnumerable<WardView>>(wardsList);

            return Task.FromResult(result);
        }
    }
}
