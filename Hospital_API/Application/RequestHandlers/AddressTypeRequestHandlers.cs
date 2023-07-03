using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddAddressTypeRequestHandler : IRequestHandler<AddAddressTypeRequest, ResponseModelView>
    {
        private readonly IAddressTypeRepository _repository;
        private readonly IMapper _mapper;

        public AddAddressTypeRequestHandler(IAddressTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddAddressTypeRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkAddressTypeExist = _repository.FindBy(x => x.Name!.Equals(request.AddressTypeDto!.Name)).FirstOrDefault();

            if (checkAddressTypeExist != null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Address Type already exist!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            AddressType addressType = new AddressType()
            {
                Name = request.AddressTypeDto!.Name,
                Description = request.AddressTypeDto.Description,
                DateCreated = currentDate,
                DateModified = currentDate,
                Active = request.AddressTypeDto.Active
            };

            _repository.Add(addressType);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<AddressType, AddressTypeView>(addressType);

            return Task.FromResult(result);
        }
    }

    public class UpdateAddressTypeRequestHandler : IRequestHandler<UpdateAddressTypeRequest, ResponseModelView>
    {
        private readonly IAddressTypeRepository _repository;
        private readonly IMapper _mapper;

        public UpdateAddressTypeRequestHandler(IAddressTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateAddressTypeRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var addressType = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (addressType == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Address Type not found!";
                result.Response = false;

                return Task.FromResult(result);
            }


            addressType.Name = request.AddressTypeDto!.Name;
            addressType.Description = request.AddressTypeDto.Description;
            addressType.DateModified = DateTime.Now;
            addressType.Active = request.AddressTypeDto.Active;

            _repository.Update(addressType);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<AddressType, AddressTypeView>(addressType);

            return Task.FromResult(result);
        }
    }

    public class GetSingleAddressTypeRequestHandler : IRequestHandler<GetSingleAddressTypeRequest, ResponseModelView>
    {
        private readonly IAddressTypeRepository _repository;
        private readonly IMapper _mapper;

        public GetSingleAddressTypeRequestHandler(IAddressTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSingleAddressTypeRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var addressType = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (addressType == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Address Type not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<AddressType, AddressTypeView>(addressType);

            return Task.FromResult(result);
        }
    }

    public class GetAllAddressTypeRequestHandler : IRequestHandler<GetAllAddressTypeRequest, ResponseModelView>
    {
        private readonly IAddressTypeRepository _repository;
        private readonly IMapper _mapper;

        public GetAllAddressTypeRequestHandler(IAddressTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllAddressTypeRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var addressTypesList = _repository.GetAll().ToList();

            if (!addressTypesList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Address Types not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<AddressType>, IEnumerable<AddressTypeView>>(addressTypesList);

            return Task.FromResult(result);
        }
    }
}
