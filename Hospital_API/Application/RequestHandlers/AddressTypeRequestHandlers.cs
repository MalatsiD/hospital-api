using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.Helpers;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            result.IsSuccessful = true;
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
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }


            addressType.Name = request.AddressTypeDto!.Name;
            addressType.Description = request.AddressTypeDto.Description;
            addressType.DateModified = DateTime.Now;
            addressType.Active = request.AddressTypeDto.Active;

            _repository.Update(addressType);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<AddressType, AddressTypeView>(addressType);

            return Task.FromResult(result);
        }
    }

    public class UpdateAddressTypeStatusRequestHandler : IRequestHandler<UpdateAddressTypeStatusRequest, ResponseModelView>
    {
        private readonly IAddressTypeRepository _repository;
        private readonly IMapper _mapper;

        public UpdateAddressTypeStatusRequestHandler(IAddressTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateAddressTypeStatusRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var addressType = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (addressType == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Address Type not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            addressType.DateModified = DateTime.Now;
            addressType.Active = request.StatusChangeDto?.Active ?? addressType.Active;

            _repository.Update(addressType);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<AddressType, AddressTypeView>(addressType);

            return Task.FromResult(result);
        }
    }

    public class DeleteAddressTypeRequestHandler : IRequestHandler<DeleteAddressTypeRequest, ResponseModelView>
    {
        private readonly IAddressTypeRepository _repository;
        private readonly IMapper _mapper;

        public DeleteAddressTypeRequestHandler(IAddressTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(DeleteAddressTypeRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var addressType = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (addressType == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Address Type not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var res = _repository.Delete(addressType);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = res;

            return Task.FromResult(result);
        }
    }

    public class CheckAddressTypeExistRequestHandler : IRequestHandler<CheckAddressTypeExistRequest, ResponseModelView>
    {
        private readonly IAddressTypeRepository _repository;
        private readonly IMapper _mapper;

        public CheckAddressTypeExistRequestHandler(IAddressTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckAddressTypeExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var addressTypeExist = _repository.FindBy(x => x.Id == request.AddressTypeId).AsNoTracking().Any();

            if (!addressTypeExist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Address Type not found!";
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

    public class CheckAddressTypeNameExistRequestHandler : IRequestHandler<CheckAddressTypeNameExistRequest, ResponseModelView>
    {
        private readonly IAddressTypeRepository _repository;
        private readonly IMapper _mapper;

        public CheckAddressTypeNameExistRequestHandler(IAddressTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckAddressTypeNameExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var AddressType= _repository.FindBy(x => x.Name!.Equals(request.Name));

            if(request.AddressTypeId > 0)
            {
                AddressType = AddressType.Where(x => x.Id != request.AddressTypeId);
            }

            var exist = AddressType.AsNoTracking().Any();

            if (exist)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Address Type already exist!";
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

            var addressType = _repository.FindBy(x => x.Id == request.Id).AsNoTracking().FirstOrDefault();

            if (addressType == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Address Type not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<AddressType, AddressTypeView>(addressType);

            return Task.FromResult(result);
        }
    }

    public class GetAllAddressTypeRequestHandler : IRequestHandler<GetAllAddressTypeRequest, ResponsePaginationModelView>
    {
        private readonly IAddressTypeRepository _repository;
        private readonly IMapper _mapper;

        public GetAllAddressTypeRequestHandler(IAddressTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponsePaginationModelView> Handle(GetAllAddressTypeRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponsePaginationModelView();

            var predicate = PredicateBuilder.True<AddressType>();

            List<AddressType>? addressTypeResult = new List<AddressType>();

            var addressTypes = _repository.GetAll().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.AddressTypeFilterDto!.Name))
            {
                predicate = predicate.And(x => x.Name!.Contains(request.AddressTypeFilterDto.Name))
                    .Or(x => x.Description!.Contains(request.AddressTypeFilterDto.Name));
            }

            addressTypes = addressTypes.Where(predicate);

            if (request.AddressTypeFilterDto.CurrentPage > 0 && request.AddressTypeFilterDto.PageSize > 0)
            {
                var pagedResult = addressTypes.GetPaged(request.AddressTypeFilterDto.CurrentPage,
                    request.AddressTypeFilterDto.PageSize);

                addressTypeResult = pagedResult.Results as List<AddressType>;
                result.CurrentPage = pagedResult.CurrentPage;
                result.PageSize = pagedResult.PageSize;
                result.TotalRecords = pagedResult.RowCount;
                result.TotalPages = pagedResult.PageCount;

            }
            else
            {
                addressTypeResult = addressTypes.ToList();
            }

            if (!addressTypeResult!.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Address Types not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<AddressType>, IEnumerable<AddressTypeView>>(addressTypeResult!);

            return Task.FromResult(result);
        }
    }
}
