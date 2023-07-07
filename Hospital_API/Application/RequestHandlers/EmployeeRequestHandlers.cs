using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Data.Repositories;
using Hospital_API.Entities;
using Hospital_API.ModelViews.EmployeeViews;
using Hospital_API.ViewModels;
using Hospital_API.ViewModels.EmployeeViews;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddEmployeePersonalInfoRequestHandler : IRequestHandler<AddEmployeePersonalInfoRequest, ResponseModelView>
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;

        public AddEmployeePersonalInfoRequestHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddEmployeePersonalInfoRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var currentDate = DateTime.Now;

            Person person = new Person()
            {
                FirstName = request.PersonalInfo!.FirstName,
                MiddleName = request.PersonalInfo!.MiddleName,
                LastName = request.PersonalInfo!.LastName,
                DateOfBirth = request.PersonalInfo!.DateOfBirth,
                Email = request.PersonalInfo!.Email,
                PhoneNumber = request.PersonalInfo!.PhoneNumber,
                TitleId = request.PersonalInfo!.TitleId,
                GenderId = request.PersonalInfo!.GenderId,
                DateCreated = currentDate,
                DateModified = currentDate,
                Active = request.PersonalInfo?.Active ?? true,
                Addresses = new Collection<PersonAddress>(),

                Employee = new Employee()
                {
                    DateCreated = currentDate,
                    DateModified = currentDate,
                    Active = request.PersonalInfo?.Active ?? true
                }
            };

            if (request.PersonalInfo!.Addresses!.Any())
            {
                foreach (var address in request.PersonalInfo.Addresses!)
                {
                    person.Addresses.Add(new PersonAddress()
                    {
                        Address = new Address()
                        {
                            AddressDetail = address.AddressDetail,
                            ZipCode = address.ZipCode,
                            CityId = address.CityId,
                            AddressTypeId = address.AddressTypeId,
                            DateCreated = currentDate,
                            DateModified = currentDate,
                            Active = address.Active
                        },
                        DateModified = currentDate,
                        Active = address.Active
                    });
                }
            }

            _repository.Add(person);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Person, EmployeeSingleView>(person);

            return Task.FromResult(result);
        }
    }

    public class AddEmploymentInfoRequestHandler : IRequestHandler<AddEmploymentInfoRequest, ResponseModelView>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public AddEmploymentInfoRequestHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddEmploymentInfoRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var employee = _repository.FindBy(x => x.Id == request.EmployeeId)
                .Include(p => p.Person)
                    .ThenInclude(t => t!.Title)
                .Include(p => p.Person)
                    .ThenInclude(g => g!.Gender)
                .Include(p => p.Person)
                    .ThenInclude(a => a!.Addresses!)
                    .ThenInclude(a => a.Address)
                    .ThenInclude(a => a!.AddressType)
                .Include(d => d!.Departments)
                .Include(m => m!.Managers)
                .Include(r => r!.Roles)
                .FirstOrDefault();

            if (employee == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Employee not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            employee.HireDate = request.EmploymentInfoDto!.EmploymentDate;

            employee.Departments = new Collection<DepartmentEmployee>()
            {
                new DepartmentEmployee()
                {
                    EmployeeId = request.EmployeeId,
                    DepartmentId = request.EmploymentInfoDto!.DepartmentId,
                    StartDate = request.EmploymentInfoDto.EmploymentDate,
                    DateCreated = currentDate,
                    DateModified = currentDate,
                    Active = employee.Active
                }
            };

            employee.Managers = new Collection<EmployeeManager>()
            {
                new EmployeeManager()
                {
                    EmployeeId = request.EmployeeId,
                    ManagerId = request.EmploymentInfoDto!.ManagerId,
                    StartDate = request.EmploymentInfoDto.EmploymentDate,
                    DateCreated= currentDate,
                    DateModified = currentDate,
                    Active = employee.Active
                }
            };

            employee.Roles = new Collection<EmployeeRole>()
            {
                new EmployeeRole()
                {
                    EmployeeId = request.EmployeeId,
                    RoleId = request.EmploymentInfoDto.RoleId,
                    StartDate= request.EmploymentInfoDto.EmploymentDate,
                    DateCreated = currentDate,
                    DateModified = currentDate,
                    Active = employee.Active
                }
            };

            var updateRes = _repository.Update(employee);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.IsSuccessful = updateRes;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Employee, EmployeeSingleView>(employee);

            return Task.FromResult(result);
        }
    }

    public class UpdateEmployeePersonalInfoRequestHandler : IRequestHandler<UpdateEmployeePersonalInfoRequest, ResponseModelView>
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;

        public UpdateEmployeePersonalInfoRequestHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateEmployeePersonalInfoRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var person = _repository.FindBy(x => x.Id == request.PersonId)
                .Include(a => a.Addresses!)
                .ThenInclude(a => a.Address)
                .Include(e => e.Employee)
                .FirstOrDefault();

            if (person == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Employee not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            person.FirstName = request.PersonalInfo!.FirstName;
            person.MiddleName = request.PersonalInfo!.MiddleName;
            person.LastName = request.PersonalInfo!.LastName;
            person.DateOfBirth = request.PersonalInfo!.DateOfBirth;
            person.Email = request.PersonalInfo!.Email;
            person.PhoneNumber = request.PersonalInfo!.PhoneNumber;
            person.TitleId = request.PersonalInfo!.TitleId;
            person.GenderId = request.PersonalInfo!.GenderId;
            person.DateModified = currentDate;
            person.Active = request.PersonalInfo?.Active ?? person.Active;

            if (person.Employee != null)
            {
                person.Employee.DateModified = currentDate;
                person.Employee.Active = request.PersonalInfo?.Active ?? person.Employee!.Active;
            }
            else
            {
                person.Employee = new Employee()
                {
                    DateCreated = currentDate,
                    DateModified = currentDate,
                    Active = request.PersonalInfo?.Active ?? true
                };
            }

            if (request.PersonalInfo!.Addresses!.Any())
            {
                foreach (var address in request.PersonalInfo.Addresses!)
                {
                    var addr = person.Addresses!.FirstOrDefault(x => x.AddressId == address.Id);

                    if (addr != null)
                    {
                        addr.Address!.AddressDetail = address.AddressDetail;
                        addr.Address.ZipCode = address.ZipCode;
                        addr.Address.CityId = address.CityId;
                        addr.Address.AddressTypeId = address.AddressTypeId;
                        addr.Address.DateModified = currentDate;
                        addr.Address.Active = person.Active;

                        addr.Active = person.Active;
                        addr.DateModified = currentDate;
                    }
                    else
                    {
                        person.Addresses?.Add(new PersonAddress
                        {
                            Person = person,
                            Address = new Address
                            {
                                AddressDetail = address.AddressDetail,
                                ZipCode = address.ZipCode,
                                DateCreated = currentDate,
                                DateModified = currentDate,
                                Active = address.Active,
                                CityId = address.CityId,
                                AddressTypeId = address.AddressTypeId
                            },
                            DateModified = currentDate,
                            Active = address.Active
                        });
                    }
                }
            }

            _repository.Update(person);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Person, EmployeeSingleView>(person);

            return Task.FromResult(result);
        }
    }

    public class ValidateEmployeePersonalInfoRequestHandler : IRequestHandler<ValidateEmployeePersonalInfoRequest, ResponseModelView>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IAddressTypeRepository _addressTypeRepository;
        private readonly ITitleRepository _titleRepository;
        private readonly IGenderRepository _genderRepository;
        private readonly IMapper _mapper;

        public ValidateEmployeePersonalInfoRequestHandler(
            ICityRepository cityRepository, 
            IAddressTypeRepository addressTypeRepository, 
            ITitleRepository titleRepository,
            IGenderRepository genderRepository,
            IMapper mapper
            )
        {
            _cityRepository = cityRepository;
            _addressTypeRepository = addressTypeRepository;
            _titleRepository = titleRepository;
            _genderRepository = genderRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(ValidateEmployeePersonalInfoRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkCity = _cityRepository.FindBy(x => request.CityIdList!.Any(id => id == x.Id)).FirstOrDefault();

            if (checkCity == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "One or more Cities not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var checkType = _addressTypeRepository.FindBy(x => request.AddressTypeIdList!.Any(id => id == x.Id)).FirstOrDefault();

            if (checkType == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "One or more Address Types not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var checkTitle = _titleRepository.FindBy(x => x.Id == request.TitleId).FirstOrDefault();

            if (checkTitle == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Title not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var checkGender = _genderRepository.FindBy(x => x.Id == request.GenderId).FirstOrDefault();

            if (checkGender == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Gender not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.IsSuccessful = true;

            return Task.FromResult(result);
        }
    }

    public class ValidateEmploymentInfoRequestHandler : IRequestHandler<ValidateEmploymentInfoRequest, ResponseModelView>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IRoleRepository _roleRepository;

        public ValidateEmploymentInfoRequestHandler(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IRoleRepository roleRepository
            )
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _roleRepository = roleRepository;
        }

        public Task<ResponseModelView> Handle(ValidateEmploymentInfoRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var managerExist = _employeeRepository.FindBy(x => x.Id == request.ManagerId).FirstOrDefault();

            if (managerExist == null )
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.IsSuccessful = false;
                result.ErrorMessage = "Manager not found!";

                return Task.FromResult(result);
            }

            var departmentExist = _departmentRepository.FindBy(x => x.Id == request.DepartmentId).FirstOrDefault();

            if (departmentExist == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.IsSuccessful = false;
                result.ErrorMessage = "Department not found!";

                return Task.FromResult(result);
            }

            var roleExist = _roleRepository.FindBy(x => x.Id == request.RoleId).FirstOrDefault();

            if (roleExist == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.IsSuccessful = false;
                result.ErrorMessage = "Role not found!";

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;

            return Task.FromResult(result);
        }
    }

    public class CheckEmployeeEmailExistRequestHandler : IRequestHandler<CheckEmployeeEmailExistRequest, ResponseModelView>
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;

        public CheckEmployeeEmailExistRequestHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckEmployeeEmailExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var employee = _repository.FindBy(x => 
                x.Email!.Equals(request.Email)
            );

            if(request.EmployeeId > 0)
            {
                employee = employee.Include(x => x.Employee).Where(x => x.Employee!.Id == request.EmployeeId);
            }

            var exist = employee.AsNoTracking().Any();

            if (exist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Employee already exist!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = exist;

            return Task.FromResult(result);
        }
    }

    public class CheckEmployeeExistRequestHandler : IRequestHandler<CheckEmployeeExistRequest, ResponseModelView>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public CheckEmployeeExistRequestHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckEmployeeExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var employeeExist = _repository.FindBy(x => x.Id == request.EmployeeId).AsNoTracking().Any();

            if (!employeeExist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Employee not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = employeeExist;

            return Task.FromResult(result);
        }
    }

    public class GetSingleEmployeeRequestHandler : IRequestHandler<GetSingleEmployeeRequest, ResponseModelView>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public GetSingleEmployeeRequestHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSingleEmployeeRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var employee = _repository.FindBy(x => x.Id == request.Id)
                .Include(d => d!.Departments!)
                    .ThenInclude(d => d.Department)
                    .ThenInclude(h => h.Hospital)
                .Include(m => m!.Managers!)
                    .ThenInclude(m => m.Manager)
                    .ThenInclude(p => p.Person)
                .Include(r => r!.Roles!)
                    .ThenInclude(r => r.Role)
                .Include(x => x.Person)
                    .ThenInclude(x => x!.Gender)
                .Include(x => x.Person)
                    .ThenInclude(x => x!.Title)
                .Include(x => x.Person)
                    .ThenInclude(x => x!.Addresses!)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x!.AddressType)
                .Include(x => x.Person)
                    .ThenInclude(x => x!.Addresses!)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x!.City)
                    .ThenInclude(x => x.Province)
                    .ThenInclude(x => x!.Country)
                .AsNoTracking().FirstOrDefault();

            if (employee == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Employee not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Employee, EmployeeSingleView>(employee);

            return Task.FromResult(result);
        }
    }

    public class GetAllEmployeeRequestHandler : IRequestHandler<GetAllEmployeeRequest, ResponseModelView>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public GetAllEmployeeRequestHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllEmployeeRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var employeesList = _repository.GetAll()
                .Include(d => d.Departments!)
                    .ThenInclude(d => d.Department)
                .Include(r => r.Roles!)
                    .ThenInclude(r => r.Role)
                .Include(x => x.Managers!)
                    .ThenInclude(x => x.Manager)
                    .ThenInclude(x => x!.Person)
                .Include(x => x.Person)
                    .ThenInclude(x => x!.Gender)
                .Include(x => x.Person)
                    .ThenInclude(x => x!.Title)
                .AsNoTracking().ToList();

            if (!employeesList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Employee not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeTableView>>(employeesList);

            return Task.FromResult(result);
        }
    }
}
