using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.Helpers;
using Hospital_API.ViewModels;
using Hospital_API.ViewModels.HospitalViews;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddHospitalRequestHandler : IRequestHandler<AddHospitalRequest, ResponseModelView>
    {
        private readonly IHospitalRepository _repository;
        private readonly IMapper _mapper;

        public AddHospitalRequestHandler(IHospitalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddHospitalRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var currentDate = DateTime.Now;

            Hospital hospital = new Hospital()
            {
                Name = request.HospitalDto!.Name,
                PhoneNumber = request.HospitalDto!.PhoneNumber,
                Email = request.HospitalDto!.Email,
                RegistrationNumber = request.HospitalDto!.RegistrationNumber,
                DateCreated = currentDate,
                DateModified = currentDate,
                Active = request.HospitalDto!.Active,
                Addresses = new Collection<HospitalAddress>()
            };

            if(request.HospitalDto!.Addresses!.Any())
            {
                foreach(var address in request.HospitalDto.Addresses!)
                {
                    hospital.Addresses.Add(new HospitalAddress
                    {
                        Hospital = hospital,
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

            _repository.Add(hospital);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Hospital, HospitalView>(hospital);

            return Task.FromResult(result);
        }
    }

    public class UpdateHospitalRequestHandler : IRequestHandler<UpdateHospitalRequest, ResponseModelView>
    {
        private readonly IHospitalRepository _repository;
        private readonly IMapper _mapper;

        public UpdateHospitalRequestHandler(IHospitalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateHospitalRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var hospital = _repository.FindBy(x => x.Id == request.Id)
                .Include(x => x.Addresses!)
                .ThenInclude(x => x.Address)
                .FirstOrDefault();

            if (hospital == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Hospital not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            hospital.Name = request.HospitalDto!.Name;
            hospital.PhoneNumber = request.HospitalDto!.PhoneNumber;
            hospital.Email = request.HospitalDto!.Email;
            hospital.RegistrationNumber = request.HospitalDto!.RegistrationNumber;
            hospital.DateModified = currentDate;
            hospital.Active = request.HospitalDto?.Active ?? hospital.Active;
            hospital.Addresses = hospital.Addresses!.Any() ? hospital.Addresses : new Collection<HospitalAddress>();

            if (request.HospitalDto!.Addresses!.Any())
            {
                foreach (var address in request.HospitalDto.Addresses!)
                {
                    var addr = hospital.Addresses?.FirstOrDefault(x => x.AddressId == address.Id);

                    if (addr != null)
                    {
                        addr.Address!.AddressDetail = address.AddressDetail;
                        addr.Address.ZipCode = address.ZipCode;
                        addr.Address.CityId = address.CityId;
                        addr.Address.AddressTypeId = address.AddressTypeId;
                        addr.Address.DateModified = currentDate;
                        addr.Address.Active = hospital.Active;

                        addr.Active = hospital.Active;
                        addr.DateModified = currentDate;
                    }
                    else
                    {
                        hospital.Addresses?.Add(new HospitalAddress
                        {
                            Hospital = hospital,
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

            _repository.Update(hospital);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Hospital, HospitalView>(hospital);

            return Task.FromResult(result);
        }
    }

    public class UpdateHospitalStatusRequestHandler : IRequestHandler<UpdateHospitalStatusRequest, ResponseModelView>
    {
        private readonly IHospitalRepository _repository;
        private readonly IMapper _mapper;

        public UpdateHospitalStatusRequestHandler(IHospitalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateHospitalStatusRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var hospital = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (hospital == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Hospital not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            hospital.DateModified = DateTime.Now;
            hospital.Active = request.StatusChangeDto?.Active ?? hospital.Active;

            _repository.Update(hospital);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Hospital, HospitalView>(hospital);

            return Task.FromResult(result);
        }
    }

    public class DeleteHospitalRequestHandler : IRequestHandler<DeleteHospitalRequest, ResponseModelView>
    {
        private readonly IHospitalRepository _repository;
        private readonly IMapper _mapper;

        public DeleteHospitalRequestHandler(IHospitalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(DeleteHospitalRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var hospital = _repository.FindBy(x => x.Id == request.Id)
                .Include(x => x.Addresses!)
                .ThenInclude(x => x.Address)
                .FirstOrDefault();

            if (hospital == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Hospital not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var res = _repository.Delete(hospital);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = res;

            return Task.FromResult(result);
        }
    }

    public class ValidateHospitalRequestHandler : IRequestHandler<ValidateHospitalRequest, ResponseModelView>
    {
        private readonly ICityRepository _repository;
        private readonly IAddressTypeRepository _typeRepository;
        private readonly IMapper _mapper;

        public ValidateHospitalRequestHandler(ICityRepository repository, IAddressTypeRepository typeRepository, IMapper mapper)
        {
            _repository = repository;
            _typeRepository = typeRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(ValidateHospitalRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkCitiesExist = _repository.GetAll().Where(x => request.CityIdList!.Any(id => id == x.Id)).ToList();

            if (checkCitiesExist.Count < request.CityIdList!.Length)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "One or more cities not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var checkAddressTypeExist = _typeRepository.GetAll().Where(x => request.AddressTypeIdList!.Any(id => id == x.Id)).ToList();

            if (checkAddressTypeExist.Count < request.AddressTypeIdList!.Length)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "One or more Address Types not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;

            return Task.FromResult(result);
        }
    }

    public class CheckHospitalExistRequestHandler : IRequestHandler<CheckHospitalExistRequest, ResponseModelView>
    {
        private readonly IHospitalRepository _repository;
        private readonly IMapper _mapper;

        public CheckHospitalExistRequestHandler(IHospitalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckHospitalExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var hospitalExist = _repository.FindBy(x => x.Id == request.HospitalId).AsNoTracking().Any();


            if (!hospitalExist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Hospital not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = hospitalExist;

            return Task.FromResult(result);
        }
    }

    public class CheckHospitalNameExistRequestHandler : IRequestHandler<CheckHospitalNameExistRequest, ResponseModelView>
    {
        private readonly IHospitalRepository _repository;
        private readonly IMapper _mapper;

        public CheckHospitalNameExistRequestHandler(IHospitalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckHospitalNameExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var hospital = _repository.FindBy(x => 
                x.Name!.Equals(request.Name)
               );

            if(request.HospitalId > 0)
            {
                hospital = hospital.Where(x => x.Id != request.HospitalId);
            }

            var exist = hospital.AsNoTracking().Any();


            if (exist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Hospital already exist!";
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

    public class GetSingleHospitalRequestHandler : IRequestHandler<GetSingleHospitalRequest, ResponseModelView>
    {
        private readonly IHospitalRepository _repository;
        private readonly IMapper _mapper;

        public GetSingleHospitalRequestHandler(IHospitalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSingleHospitalRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var hospital = _repository.FindBy(x => x.Id == request.Id)
                .Include(x => x.Addresses!)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x!.AddressType)
                .Include(x => x.Addresses!)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x!.City)
                .ThenInclude(x => x!.Province)
                .ThenInclude(x => x!.Country)
                .AsNoTracking().FirstOrDefault();
                

            if (hospital == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Hospital not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Hospital, HospitalView>(hospital);

            return Task.FromResult(result);
        }
    }

    public class GetAllHospitalListRequestHandler : IRequestHandler<GetAllHospitalListRequest, ResponseModelView>
    {
        private readonly IHospitalRepository _repository;
        private readonly IMapper _mapper;

        public GetAllHospitalListRequestHandler(IHospitalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllHospitalListRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var hospitals = _repository.FindBy(x => x.Active == request.Active).AsNoTracking().ToList();

            if (!hospitals.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Hospitals not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Hospital>, IEnumerable<HospitalListView>>(hospitals);

            return Task.FromResult(result);
        }
    }

    public class GetAllHospitalRequestHandler : IRequestHandler<GetAllHospitalRequest, ResponsePaginationModelView>
    {
        private readonly IHospitalRepository _repository;
        private readonly IMapper _mapper;

        public GetAllHospitalRequestHandler(IHospitalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponsePaginationModelView> Handle(GetAllHospitalRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponsePaginationModelView();

            var predicate = PredicateBuilder.True<Hospital>();

            List<Hospital>? hospitalResult = new List<Hospital>();

            var hospitals = _repository.GetAll()
                .Include(x => x.Addresses!)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x!.AddressType)
                .Include(x => x.Addresses!)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x!.City)
                .ThenInclude(x => x!.Province)
                .ThenInclude(x => x!.Country)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.HospitalFilterDto!.Name))
            {
                predicate = predicate.And(x => x.Name!.Contains(request.HospitalFilterDto.Name))
                    .Or(x => x.Name!.Contains(request.HospitalFilterDto.Name));
            }
            if (!string.IsNullOrWhiteSpace(request.HospitalFilterDto.RegistrationNumber))
            {
                predicate = predicate.Or(x => x.RegistrationNumber!.Contains(request.HospitalFilterDto.RegistrationNumber));
            }

            hospitals = hospitals.Where(predicate);

            if (request.HospitalFilterDto.CurrentPage > 0 && request.HospitalFilterDto.PageSize > 0)
            {
                var pagedResult = hospitals.GetPaged(request.HospitalFilterDto.CurrentPage,
                    request.HospitalFilterDto.PageSize);

                hospitalResult = pagedResult.Results as List<Hospital>;
                result.CurrentPage = pagedResult.CurrentPage;
                result.PageSize = pagedResult.PageSize;
                result.TotalRecords = pagedResult.RowCount;
                result.TotalPages = pagedResult.PageCount;

            }
            else
            {
                hospitalResult = hospitals.ToList();
            }


            if (hospitalResult == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Hospitals not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Hospital>, IEnumerable<HospitalView>>(hospitalResult);

            return Task.FromResult(result);
        }
    }
}
