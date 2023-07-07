using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Data.Repositories;
using Hospital_API.DTOs.Patient;
using Hospital_API.Entities;
using Hospital_API.ModelViews.PatientViews;
using Hospital_API.ViewModels;
using Hospital_API.ViewModels.PatientViews;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddPatientPersonalInfoRequestHandler : IRequestHandler<AddPatientPersonalInfoRequest, ResponseModelView>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public AddPatientPersonalInfoRequestHandler(
            IPersonRepository personRepository,
            IMapper mapper)
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddPatientPersonalInfoRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkPatientExist = _personRepository.GetAll()
                .Include(x => x.Patient)
                .Where(x => x.Patient!.PatientNumber == request.PatientPersonalInfo!.PatientNumber)
                .FirstOrDefault();

            if (checkPatientExist != null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Patient already exist!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            Person person = new Person()
            {
                FirstName = request.PatientPersonalInfo!.FirstName,
                MiddleName = request.PatientPersonalInfo!.MiddleName,
                LastName = request.PatientPersonalInfo!.LastName,
                DateOfBirth = request.PatientPersonalInfo!.DateOfBirth,
                Email = request.PatientPersonalInfo!.Email,
                PhoneNumber = request.PatientPersonalInfo!.PhoneNumber,
                TitleId = request.PatientPersonalInfo!.TitleId,
                GenderId = request.PatientPersonalInfo!.GenderId,
                DateCreated = currentDate,
                DateModified = currentDate,
                Active = request.PatientPersonalInfo!.Active,

                Patient = new Patient()
                {
                    PatientNumber = request.PatientPersonalInfo!.PatientNumber,
                    DateCreated = currentDate,
                    DateModified = currentDate,
                    Active = request.PatientPersonalInfo.Active
                },

                Addresses = new Collection<PersonAddress>()
            };

            if (request.PatientPersonalInfo.Addresses!.Any())
            {
                foreach (var address in request.PatientPersonalInfo.Addresses!)
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
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Person, PatientSingleView>(person);

            return Task.FromResult(result);
        }
    }

    public class AddPatientAdmitRequestHandler : IRequestHandler<AddPatientAdmitRequest, ResponseModelView>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IWardRepository _wardRepository;
        private readonly IMapper _mapper;

        public AddPatientAdmitRequestHandler(
            IPatientRepository patientRepository, 
            IWardRepository wardRepository,
            IMapper mapper
            )
        {
            _patientRepository = patientRepository;
            _wardRepository = wardRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddPatientAdmitRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkWard = _wardRepository.FindBy(x => x.Id == request.PatientAdmitDto!.WardId).FirstOrDefault();

            if (checkWard == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Ward not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var patient = _patientRepository.FindBy(x => x.Id == request.PatientId)
                .Include(x => x.Person)
                    .ThenInclude(x => x!.Gender)
                .Include(x => x.Person)
                    .ThenInclude(x => x!.Title)
                .Include(x => x.PatientAdmits)
                .FirstOrDefault();

            if(patient == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Patient not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            patient.PatientAdmits = new Collection<PatientAdmit>()
            {
                new PatientAdmit()
                {
                    AdmitDate = request.PatientAdmitDto?.AdmitDate ?? currentDate,
                    AdmitComment = request.PatientAdmitDto!.AdmitComment,
                    PatientId = patient.Id,
                    WardId = request.PatientAdmitDto.WardId,
                    DateCreated = currentDate,
                    DateModified = currentDate,
                    Active = request.PatientAdmitDto?.Active ?? patient.Active,
                    Ailments = GetAdmitAilment(request.PatientAdmitDto!.AdmitAilmentDtos!)
                }
            };

            _patientRepository.Update(patient);
            _patientRepository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Patient, PatientSingleView>(patient);

            return Task.FromResult(result);
        }

        private Collection<AdmitAilment> GetAdmitAilment(ICollection<AdmitAilmentDto> admitAilmentDto)
        {
            var result = new Collection<AdmitAilment>();

            var currentDate = DateTime.Now;

            foreach(var admitAilment in  admitAilmentDto)
            {
                result.Add(new AdmitAilment
                {
                    AilmentId = admitAilment.AilmentId,
                    DateCreated = currentDate,
                    DateModified = currentDate,
                    Active = admitAilment.Active,
                });
            }

            return result;
        }
    }

    public class UpdatePatientAdmitRequestHandler : IRequestHandler<UpdatePatientAdmitRequest, ResponseModelView>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IWardRepository _wardRepository;
        private readonly IMapper _mapper;

        public UpdatePatientAdmitRequestHandler(
            IPatientRepository patientRepository,
            IWardRepository wardRepository,
            IMapper mapper
            )
        {
            _patientRepository = patientRepository;
            _wardRepository = wardRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdatePatientAdmitRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkWard = _wardRepository.FindBy(x => x.Id == request.PatientAdmitDto!.WardId).FirstOrDefault();

            if (checkWard == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Ward not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var patient = _patientRepository.FindBy(x => x.Id == request.PatientId)
                .Include(x => x.Person)
                    .ThenInclude(x => x!.Gender)
                .Include(x => x.Person)
                    .ThenInclude(x => x!.Title)
                .Include(x => x.PatientAdmits)
                .FirstOrDefault();

            if (patient == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Patient not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            if(patient.PatientAdmits!.Any())
            {
                var editAdmit = patient.PatientAdmits!.FirstOrDefault(x => x.Id == request.PatientAdmitId);

                if(editAdmit != null)
                {
                    editAdmit.AdmitDate = request.PatientAdmitDto!.AdmitDate;
                    editAdmit.AdmitComment = request.PatientAdmitDto!.AdmitComment;
                    editAdmit.WardId = request.PatientAdmitDto!.WardId;
                    editAdmit.DateModified = currentDate;
                    editAdmit.Ailments = editAdmit.Ailments!.Any() ? editAdmit.Ailments : new Collection<AdmitAilment>();

                    foreach (var ailment in request.PatientAdmitDto.AdmitAilmentDtos!)
                    {
                        editAdmit.Ailments!.Add(new AdmitAilment()
                        {
                            AilmentId = ailment.AilmentId,
                            DateCreated = currentDate,
                            DateModified = currentDate,
                            Active = request.PatientAdmitDto?.Active ?? true
                        });
                    }
                    
                }
            }

            _patientRepository.Update(patient);
            _patientRepository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Patient, PatientSingleView>(patient);

            return Task.FromResult(result);
        }
    }

    public class UpdatePatientPersonalInfoRequestHandler : IRequestHandler<UpdatePatientPersonalInfoRequest, ResponseModelView>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public UpdatePatientPersonalInfoRequestHandler(
            IPersonRepository personRepository,
            IMapper mapper
            )
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdatePatientPersonalInfoRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var person = _personRepository.FindBy(x => x.Id == request.PersonId)
                .Include(x => x.Patient)
                .Include(x => x.Addresses!)
                .ThenInclude(x => x.Address)
                .FirstOrDefault();

            if (person == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Patient not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            person.FirstName = request.PatientPersonalInfo!.FirstName;
            person.MiddleName = request.PatientPersonalInfo!.MiddleName;
            person.LastName = request.PatientPersonalInfo!.LastName;
            person.DateOfBirth = request.PatientPersonalInfo!.DateOfBirth;
            person.Email = request.PatientPersonalInfo!.Email;
            person.PhoneNumber = request.PatientPersonalInfo!.PhoneNumber;
            person.TitleId = request.PatientPersonalInfo!.TitleId;
            person.GenderId = request.PatientPersonalInfo!.GenderId;
            person.DateModified = currentDate;
            person.Active = request.PatientPersonalInfo?.Active ?? person.Active;

            if(person.Patient != null)
            {
                person.Patient.PatientNumber = request.PatientPersonalInfo!.PatientNumber;
                person.Patient.DateModified = currentDate;
                person.Patient.Active = request.PatientPersonalInfo?.Active ?? person.Active;
            }
            else
            {
                person.Patient = new Patient()
                {
                    PatientNumber = request.PatientPersonalInfo!.PatientNumber,
                    DateModified = currentDate,
                    DateCreated = currentDate,
                    Active = request.PatientPersonalInfo?.Active ?? person.Active
                };
            }

            if(request.PatientPersonalInfo!.Addresses!.Any())
            {
                foreach(var address in request.PatientPersonalInfo.Addresses!)
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

            _personRepository.Update(person);
            _personRepository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Person, PatientSingleView>(person);

            return Task.FromResult(result);
        }
    }

    public class AddPatientTransferRequestHandler : IRequestHandler<AddPatientTransferRequest, ResponseModelView>
    {
        private readonly IPatientAdmitRepository _patientAdmitRepository;
        private readonly IMapper _mapper;

        public AddPatientTransferRequestHandler(
            IPatientAdmitRepository patientAdmitRepository,
            IMapper mapper
            )
        {
            _patientAdmitRepository = patientAdmitRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddPatientTransferRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var patientAdmit = _patientAdmitRepository.FindBy(x => x.Id == request.PatientAdmitId).FirstOrDefault();

            if(patientAdmit == null )
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.IsSuccessful = false;
                result.ErrorMessage = "Admit not found!";

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            patientAdmit.PatientTranfers = patientAdmit.PatientTranfers!.Any() ? patientAdmit.PatientTranfers : new Collection<PatientTranfer>();

            patientAdmit.PatientTranfers!.Add(new PatientTranfer()
            {
                TransferDate = request.PatientTranferDto?.TransferDate ?? currentDate,
                Reason = request.PatientTranferDto!.Reason,
                HospitalId = request.PatientTranferDto!.HospitalId,
                WardId = request.PatientTranferDto.WardId,
                DateCreated = currentDate,
                DateModified = currentDate,
                Active = request.PatientTranferDto?.Active ?? true
            });

            _patientAdmitRepository.Update(patientAdmit);
            _patientAdmitRepository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<PatientAdmit, PatientAdmitView>(patientAdmit);

            return Task.FromResult(result);
        }
    }

    public class ValidatePatientRequestHandler : IRequestHandler<ValidatePatientPersonalInfoRequest, ResponseModelView>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IAddressTypeRepository _addressTypeRepository;
        private readonly ITitleRepository _titleRepository;
        private readonly IGenderRepository _genderRepository;
        private readonly IMapper _mapper;

        public ValidatePatientRequestHandler(
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

        public Task<ResponseModelView> Handle(ValidatePatientPersonalInfoRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkCityExist = _cityRepository.FindBy(x => request.CityIdList!.Any(id => id == x.Id)).FirstOrDefault();

            if (checkCityExist == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "One or more cities not found!";
                result.IsSuccessful = false;

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
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = true;

            return Task.FromResult(result);
        }
    }

    public class CheckPatientExistRequestHandler : IRequestHandler<CheckPatientExistRequest, ResponseModelView>
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;

        public CheckPatientExistRequestHandler(
            IPatientRepository repository,
            IMapper mapper
            )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckPatientExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var patientExist = _repository.FindBy(x => x.Id == request.PatientId).AsNoTracking().Any();

            if (!patientExist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Patient not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = patientExist;

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
                    .ThenInclude(x => x!.AddressType)
                .Include(x => x.Person)
                    .ThenInclude(x => x!.Addresses!)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x!.City)
                    .ThenInclude(x => x.Province)
                    .ThenInclude(x => x!.Country)
                .Include(x => x.PatientAdmits!)
                    .ThenInclude(x => x.Ailments!)
                    .ThenInclude(x => x.Ailment)
                .AsNoTracking().FirstOrDefault();

            if (patient == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Patient not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Patient, PatientSingleView>(patient);

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
                .AsNoTracking().ToList();

            if (!patientsList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Patients not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Patient>, IEnumerable<PatientTableView>>(patientsList);

            return Task.FromResult(result);
        }
    }
}
