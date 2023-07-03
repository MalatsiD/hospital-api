using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Data.Repositories;
using Hospital_API.Entities;
using Hospital_API.ModelViews.EmployeeViews;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddEmployeeRequestHandler : IRequestHandler<AddEmployeeRequest, ResponseModelView>
    {
        private readonly IPersonRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public AddEmployeeRequestHandler(IPersonRepository repository, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddEmployeeRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkEmployeeExist = _repository.FindBy(x => x.Email!.Equals(request.EmployeeDto!.Email)).FirstOrDefault();

            if (checkEmployeeExist != null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Employee not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var manager = _employeeRepository.FindBy(x => x.Id == request.EmployeeDto!.ManagerId)
                .Include(x => x.Person)
                .FirstOrDefault();

            if (manager == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Manager not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            Person person = new Person()
            {
                FirstName = request.EmployeeDto!.FirstName,
                MiddleName = request.EmployeeDto!.MiddleName,
                LastName = request.EmployeeDto!.LastName,
                DateOfBirth = request.EmployeeDto!.DateOfBirth,
                Email = request.EmployeeDto!.Email,
                PhoneNumber = request.EmployeeDto!.PhoneNumber,
                TitleId = request.EmployeeDto!.TitleId,
                GenderId = request.EmployeeDto!.GenderId,
                DateCreated = currentDate,
                DateModified = currentDate,
                Active = request.EmployeeDto?.Active ?? true,

                Employee = new Employee()
                {
                    HireDate = request.EmployeeDto?.HireDate ?? DateTime.Now,
                    TerminationDate = request.EmployeeDto?.TerminationDate ?? DateTime.Now.AddYears(40),
                    DateCreated = currentDate,
                    DateModified = currentDate,
                    Active = request.EmployeeDto?.Active ?? true,

                    Managers = new Collection<EmployeeManager>()
                    {
                        new EmployeeManager()
                        {
                            Manager = manager,
                            ManagerId = request.EmployeeDto!.ManagerId,
                            StartDate = request.EmployeeDto.ManageStartDate,
                            EndDate = request.EmployeeDto.ManageEndDate,
                            DateCreated = currentDate,
                            DateModified= currentDate,
                            Active= request.EmployeeDto?.Active ?? true,
                        }
                    }
                },

                Addresses = new Collection<PersonAddress>()
            };

            if(request.EmployeeDto!.Addresses!.Any())
            {
                foreach(var address in request.EmployeeDto.Addresses!)
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
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Person, EmployeeView>(person);

            return Task.FromResult(result);
        }
    }

    public class UpdateEmployeeRequestHandler : IRequestHandler<UpdateEmployeeRequest, ResponseModelView>
    {
        private readonly IPersonRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public UpdateEmployeeRequestHandler(IPersonRepository repository, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateEmployeeRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var person = _repository.FindBy(x => x.Id == request.Id)
                .Include(x => x.Employee)
                .ThenInclude(x => x!.Managers!)
                .ThenInclude(x => x.Manager)
                .Include(x => x.Addresses!)
                .ThenInclude(x => x.Address)
                .FirstOrDefault();

            if (person == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Employee not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var manager = _employeeRepository.FindBy(x => x.Id == request.EmployeeDto!.ManagerId)
                .Include(x => x.Person)
                .FirstOrDefault();

            if (manager == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Manager not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            person.FirstName = request.EmployeeDto!.FirstName;
            person.MiddleName = request.EmployeeDto!.MiddleName;
            person.LastName = request.EmployeeDto!.LastName;
            person.DateOfBirth = request.EmployeeDto!.DateOfBirth;
            person.Email = request.EmployeeDto!.Email;
            person.PhoneNumber = request.EmployeeDto!.PhoneNumber;
            person.TitleId = request.EmployeeDto!.TitleId;
            person.GenderId = request.EmployeeDto!.GenderId;
            person.DateModified = currentDate;
            person.Active = request.EmployeeDto?.Active ?? person.Active;

            if(person.Employee != null)
            {
                person.Employee.HireDate = request.EmployeeDto?.HireDate ?? DateTime.Now;
                person.Employee.TerminationDate = request.EmployeeDto?.TerminationDate ?? person.Employee!.TerminationDate;
                person.Employee.DateModified = currentDate;
                person.Employee.Active = request.EmployeeDto?.Active ?? person.Employee!.Active;
            }
            else
            {
                person.Employee = new Employee()
                {
                    HireDate = request.EmployeeDto?.HireDate ?? DateTime.Now,
                    TerminationDate = request.EmployeeDto?.TerminationDate ?? DateTime.Now.AddYears(40),
                    DateCreated = currentDate,
                    DateModified = currentDate,
                    Active = request.EmployeeDto?.Active ?? true
                };
            }

            

            if (request.EmployeeDto!.Addresses!.Any())
            {
                foreach (var address in request.EmployeeDto.Addresses!)
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

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Person, EmployeeView>(person);

            return Task.FromResult(result);
        }
    }

    public class ValidateEmployeeRequestHandler : IRequestHandler<ValidateEmployeeRequest, ResponseModelView>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IAddressTypeRepository _addressTypeRepository;
        private readonly IMapper _mapper;

        public ValidateEmployeeRequestHandler(
            ICityRepository cityRepository, 
            IAddressTypeRepository addressTypeRepository, 
            IMapper mapper
            )
        {
            _cityRepository = cityRepository;
            _addressTypeRepository = addressTypeRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(ValidateEmployeeRequest request, CancellationToken cancellationToken)
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
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = true;

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
                .Include(x => x.Managers!)
                .ThenInclude(x => x.Manager)
                .ThenInclude(x => x!.Person)
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
                .FirstOrDefault();

            if (employee == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Employee not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Employee, EmployeeView>(employee);

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
                .Include(x => x.Managers!)
                .ThenInclude(x => x.Manager)
                .ThenInclude(x => x!.Person)
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
                .ToList();

            if (!employeesList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Employee not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeView>>(employeesList);

            return Task.FromResult(result);
        }
    }
}
