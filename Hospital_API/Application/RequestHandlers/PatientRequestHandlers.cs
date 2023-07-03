using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.ModelViews.PatientViews;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddPatientRequestHandler : IRequestHandler<AddPatientRequest, ResponseModelView>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public AddPatientRequestHandler( 
            IPersonRepository personRepository, 
            IMapper mapper
            )
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddPatientRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkPatientExist = _personRepository.GetAll()
                .Include(x => x.Patient)
                .Where(x => x.Patient!.PatientNumber == request.PatientDto!.PatientNumber)
                .FirstOrDefault();

            if ( checkPatientExist != null )
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Patient already exist!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            Person person = new Person()
            {
                FirstName = request.PatientDto!.FirstName,
                MiddleName = request.PatientDto!.MiddleName,
                LastName = request.PatientDto!.LastName,
                DateOfBirth = request.PatientDto!.DateOfBirth,
                Email = request.PatientDto!.Email,
                PhoneNumber = request.PatientDto!.PhoneNumber,
                TitleId = request.PatientDto!.TitleId,
                GenderId = request.PatientDto!.GenderId,
                DateCreated = currentDate,
                DateModified = currentDate,
                Active = request.PatientDto!.Active,

                Patient = new Patient()
                {
                    PatientNumber = request.PatientDto!.PatientNumber,
                    DateCreated = currentDate,
                    DateModified = currentDate,
                    Active = request.PatientDto.Active
                },

                Addresses = new Collection<PersonAddress>()
            };

            if(request.PatientDto.Addresses!.Any())
            {
                foreach(var address in request.PatientDto.Addresses!)
                {
                    person.Addresses.Add(new PersonAddress()
                    {
                        Address = new Address()
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
                    }
                    );
                }
            }

            _personRepository.Add(person);
            _personRepository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Person, PatientView>(person);

            return Task.FromResult( result );
        }
    }

    public class UpdatePatientRequestHandler : IRequestHandler<UpdatePatientRequest, ResponseModelView>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public UpdatePatientRequestHandler(
            IPersonRepository personRepository,
            IMapper mapper
            )
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdatePatientRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var person = _personRepository.FindBy(x => x.Id == request.Id)
                .Include(x => x.Patient)
                .Include(x => x.Addresses!)
                .ThenInclude(x => x.Address)
                .FirstOrDefault();

            if (person == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Patient not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            person.FirstName = request.PatientDto!.FirstName;
            person.MiddleName = request.PatientDto!.MiddleName;
            person.LastName = request.PatientDto!.LastName;
            person.DateOfBirth = request.PatientDto!.DateOfBirth;
            person.Email = request.PatientDto!.Email;
            person.PhoneNumber = request.PatientDto!.PhoneNumber;
            person.TitleId = request.PatientDto!.TitleId;
            person.GenderId = request.PatientDto!.GenderId;
            person.DateModified = currentDate;
            person.Active = request.PatientDto?.Active ?? person.Active;

            if(person.Patient != null)
            {
                person.Patient.PatientNumber = request.PatientDto!.PatientNumber;
                person.Patient.DateModified = currentDate;
                person.Patient.Active = request.PatientDto?.Active ?? person.Active;
            }
            else
            {
                person.Patient = new Patient()
                {
                    PatientNumber = request.PatientDto!.PatientNumber,
                    DateModified = currentDate,
                    DateCreated = currentDate,
                    Active = request.PatientDto?.Active ?? person.Active
                };
            }

            if(request.PatientDto!.Addresses!.Any())
            {
                foreach(var address in request.PatientDto.Addresses!)
                {
                    var addr = person.Addresses!.FirstOrDefault(x => x.AddressId == address.Id);

                    if (addr != null)
                    {
                        addr.Address.AddressDetail = address.AddressDetail;
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

            _personRepository.Update(person);
            _personRepository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Person, PatientView>(person);

            return Task.FromResult(result);
        }
    }

    public class ValidatePatientRequestHandler : IRequestHandler<ValidatePatientRequest, ResponseModelView>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IAddressTypeRepository _addressTypeRepository;
        private readonly IMapper _mapper;

        public ValidatePatientRequestHandler(
            ICityRepository cityRepository,
            IAddressTypeRepository addressTypeRepository,
            IMapper mapper
            )
        {
            _cityRepository = cityRepository;
            _addressTypeRepository = addressTypeRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(ValidatePatientRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkCityExist = _cityRepository.FindBy(x => request.CityIdList!.Any(id => id == x.Id)).FirstOrDefault();

            if (checkCityExist == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "One or more cities not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var checkTypeExist = _addressTypeRepository.FindBy(x => request.AddressTypeIdList!.Any(id => id == x.Id)).FirstOrDefault();

            if (checkTypeExist == null)
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

    public class GetSinglePatientRequestHandler : IRequestHandler<GetSinglePatientRequest, ResponseModelView>
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;

        public GetSinglePatientRequestHandler(
            IPatientRepository repository,
            IMapper mapper
            )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSinglePatientRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var patient = _repository.FindBy(x => x.Id == request.Id)
                 .Include(x => x.Person)
                .ThenInclude(x => x!.Gender)
                .Include(x => x.Person)
                .ThenInclude(x => x!.Title)
                .Include(x => x.Person)
                .ThenInclude(x => x!.Addresses!)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x.AddressType)
                .Include(x => x.Person)
                .ThenInclude(x => x!.Addresses!)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x.City)
                .ThenInclude(x => x.Province)
                .ThenInclude(x => x!.Country)
                .FirstOrDefault();

            if (patient == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Patient not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Patient, PatientView>(patient);

            return Task.FromResult(result);
        }
    }

    public class GetAllPatientRequestHandler : IRequestHandler<GetAllPatientRequest, ResponseModelView>
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPatientRequestHandler(
            IPatientRepository repository,
            IMapper mapper
            )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllPatientRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var patientsList = _repository.GetAll()
                .Include(x => x.Person)
                .ThenInclude(x => x!.Gender)
                .Include(x => x.Person)
                .ThenInclude(x => x!.Title)
                .Include(x => x.Person)
                .ThenInclude(x => x!.Addresses!)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x.AddressType)
                .Include(x => x.Person)
                .ThenInclude(x => x!.Addresses!)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x.City)
                .ThenInclude(x => x.Province)
                .ThenInclude(x => x!.Country)
                .ToList();

            if (!patientsList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Patients not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Patient>, IEnumerable<PatientView>>(patientsList);

            return Task.FromResult(result);
        }
    }
}
