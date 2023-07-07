using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Data.Repositories;
using Hospital_API.Entities;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddDepartmentRequestHandler : IRequestHandler<AddDepartmentRequest, ResponseModelView>
    {
        private readonly IDepartmentRepository _repository;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IMapper _mapper;

        public AddDepartmentRequestHandler(IDepartmentRepository repository, IHospitalRepository hospitalRepository, IMapper mapper)
        {
            _repository = repository;
            _hospitalRepository = hospitalRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddDepartmentRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var currentDate = DateTime.Now;

            Department department = new Department()
            {
                Name = request.DepartmentDto!.Name,
                Code = request.DepartmentDto?.Code,
                Description = request.DepartmentDto?.Description,
                DateCreated = currentDate,
                DateModified = currentDate,
                Active = request.DepartmentDto?.Active ?? true,
                HospitalId = request.DepartmentDto!.HospitalId
            };

            _repository.Add(department);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Department, DepartmentView>(department);

            return Task.FromResult(result);
        }
    }

    public class UpdateDepartmentRequestHandler : IRequestHandler<UpdateDepartmentRequest, ResponseModelView>
    {
        private readonly IDepartmentRepository _repository;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IMapper _mapper;

        public UpdateDepartmentRequestHandler(IDepartmentRepository repository, IHospitalRepository hospitalRepository, IMapper mapper)
        {
            _repository = repository;
            _hospitalRepository = hospitalRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateDepartmentRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkHospitalExist = _hospitalRepository.FindBy(x => x.Id == request.DepartmentDto!.HospitalId).FirstOrDefault();

            if (checkHospitalExist == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.IsSuccessful = false;
                result.ErrorMessage = "Hospital not found!";

                return Task.FromResult(result);
            }

            var department = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (department == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.IsSuccessful = false;
                result.ErrorMessage = "Department not found!";

                return Task.FromResult(result);
            }

            department.Name = request.DepartmentDto!.Name;
            department.Code = request.DepartmentDto?.Code;
            department.Description = request.DepartmentDto?.Description;
            department.DateModified = DateTime.Now;
            department.Active = request.DepartmentDto?.Active ?? department.Active;
            department.HospitalId = request.DepartmentDto!.HospitalId;

            _repository.Update(department);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Department, DepartmentView>(department);

            return Task.FromResult(result);
        }
    }

    public class CheckDepartmentExistRequestHandler : IRequestHandler<CheckDepartmentExistRequest, ResponseModelView>
    {
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;

        public CheckDepartmentExistRequestHandler(IDepartmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckDepartmentExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var departmentExist = _repository.FindBy(x => x.Id == request.DepartmentId).AsNoTracking().Any();

            if (!departmentExist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.IsSuccessful = false;
                result.ErrorMessage = "Department not found!";

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = departmentExist;

            return Task.FromResult(result);
        }
    }

    public class CheckDepartmentNameExistRequestHandler : IRequestHandler<CheckDepartmentNameExistRequest, ResponseModelView>
    {
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;

        public CheckDepartmentNameExistRequestHandler(IDepartmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckDepartmentNameExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var department = _repository.FindBy(x => 
                    x.Name!.Equals(request.Name) &&
                    x.HospitalId == request.HospitalId
                );

            if(request.DepartmentId > 0)
            {
                department = department.Where(x => x.Id != request.DepartmentId);
            }

            var exist = department.AsNoTracking().Any();

            if (exist)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.IsSuccessful = false;
                result.ErrorMessage = "Department already exist!";

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = exist;

            return Task.FromResult(result);
        }
    }

    public class GetSingleDepartmentRequestHandler : IRequestHandler<GetSingleDepartmentRequest, ResponseModelView>
    {
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;

        public GetSingleDepartmentRequestHandler(IDepartmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSingleDepartmentRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var department = _repository.FindBy(x => x.Id == request.Id)
                .Include(x => x.Hospital)
                .AsNoTracking().FirstOrDefault();

            if (department == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.IsSuccessful = false;
                result.ErrorMessage = "Department not found!";

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Department, DepartmentView>(department);

            return Task.FromResult(result);
        }
    }

    public class GetAllDepartmentRequestHandler : IRequestHandler<GetAllDepartmentRequest, ResponseModelView>
    {
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;

        public GetAllDepartmentRequestHandler(IDepartmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllDepartmentRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var departmentsList = _repository.GetAll()
                .Include(x => x.Hospital)
                .AsNoTracking().ToList();

            if (!departmentsList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.IsSuccessful = false;
                result.ErrorMessage = "Departments not found!";

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentView>>(departmentsList);

            return Task.FromResult(result);
        }
    }
}
