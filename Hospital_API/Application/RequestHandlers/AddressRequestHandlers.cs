using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.Enums;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Hospital_API.Application.RequestHandlers
{

    public class AddAddressRequestHandler : IRequestHandler<AddAddressRequest, ResponseModelView>
    {
        private readonly IAddressRepository _repository;
        private readonly IMapper _mapper;

        public AddAddressRequestHandler(IAddressRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddAddressRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            DateTime currentDate = DateTime.Now;

            var address = new Address
            {
                AddressDetail = request.AddressDto!.AddressDetail,
                ZipCode = request.AddressDto.ZipCode,
                CityId = request.AddressDto.CityId,
                AddressTypeId = request.AddressDto.AddressTypeId,
                DateCreated = currentDate,
                DateModified = currentDate,
                Active = request.AddressDto.Active
            };

            switch (request.AddressDto.AddressFor)
            {
                case (short)AddressFor.Person:
                    address.People = new Collection<PersonAddress>()
                    {
                        new PersonAddress()
                        {
                            PersonId = request.AddressDto.PersonAddressDto!.PersonId,
                            DateModified = currentDate,
                            Active = request.AddressDto?.Active ?? true
                        }
                    };
                    break;
                case (short)AddressFor.Hospital:
                    address.Hospitals = new Collection<HospitalAddress>()
                    {
                        new HospitalAddress()
                        {
                            HospitalId = request.AddressDto.HospitalAddressDto!.HospitalId,
                            DateModified = currentDate,
                            Active = request.AddressDto?.Active ?? true
                        }
                    };
                    break;
            }

            _repository.Add(address);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.ErrorMessage = null;
            result.IsSuccessful = true;
            result.Response = _mapper.Map<Address, AddressView>(address);

            return Task.FromResult(result);
        }
    }

    public class UpdateAddressRequestHandler : IRequestHandler<UpdateAddressRequest, ResponseModelView>
    {
        private readonly IAddressRepository _repository;
        private readonly IMapper _mapper;

        public UpdateAddressRequestHandler(IAddressRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateAddressRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            DateTime currentDate = DateTime.Now;

            var address = _repository.FindBy(x => x.Id == request.Id).AsNoTracking().FirstOrDefault();

            if(address == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Address not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            address.AddressDetail = request.UpdateAddressDto!.AddressDetail;
            address.ZipCode = request.UpdateAddressDto.ZipCode;
            address.CityId = request.UpdateAddressDto.CityId;
            address.AddressTypeId = request.UpdateAddressDto.AddressTypeId;
            address.DateModified = currentDate;

            _repository.Update(address);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.IsSuccessful = true;
            result.Response = _mapper.Map<Address, AddressView>(address);

            return Task.FromResult(result);
        }
    }

    public class CheckCityInAddressExistRequestHandler : IRequestHandler<CheckCityInAddressExistRequest, ResponseModelView>
    {
        private readonly IAddressRepository _addressRepository;

        public CheckCityInAddressExistRequestHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public Task<ResponseModelView> Handle(CheckCityInAddressExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var cityExist = _addressRepository.FindBy(x => x.CityId == request.CityId).AsNoTracking().Any();

            if(cityExist)
            {
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = "City cannot be deleted!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = cityExist;

            return Task.FromResult(result);
        }
    }

    public class CheckAddressTypeInAddressExistRequestHandler : IRequestHandler<CheckAddressTypeInAddressExistRequest, ResponseModelView>
    {
        private readonly IAddressRepository _addressRepository;

        public CheckAddressTypeInAddressExistRequestHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public Task<ResponseModelView> Handle(CheckAddressTypeInAddressExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var addressTypeExist = _addressRepository.FindBy(x => x.AddressTypeId == request.AddressTypeId).AsNoTracking().Any();

            if (addressTypeExist)
            {
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = "Address Type cannot be deleted!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = addressTypeExist;

            return Task.FromResult(result);
        }
    }
}
