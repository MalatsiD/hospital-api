using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddPharmaceuticalCategoryRequestHandler :
        IRequestHandler<AddPharmaceuticalCategoryRequest, ResponseModelView>
    {
        private readonly IPharmaceuticalCategoryRepository _repository;
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        public AddPharmaceuticalCategoryRequestHandler(
            IPharmaceuticalCategoryRepository repository,
            IVendorRepository vendorRepository,
            IMapper mapper
            )
        {
            _repository = repository;
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddPharmaceuticalCategoryRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkPharmaceuticalCatExist = _repository.FindBy(x => 
                x.Name!.Equals(request.PharmaceuticalCategoryDto!.Name) &&
                x.VendorId == request.PharmaceuticalCategoryDto.VendorId
            ).FirstOrDefault();

            if (checkPharmaceuticalCatExist != null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Pharmaceutical Category already exist!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            PharmaceuticalCategory category = new PharmaceuticalCategory()
            {
                Name = request.PharmaceuticalCategoryDto!.Name,
                Description = request.PharmaceuticalCategoryDto!.Description,
                DateCreated = currentDate,
                DateModified = currentDate,
                Active = request.PharmaceuticalCategoryDto?.Active ?? true,
                VendorId = request.PharmaceuticalCategoryDto!.VendorId,
            };

            _repository.Add(category);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<PharmaceuticalCategory, PharmaceuticalCategoryView>(category);

            return Task.FromResult( result );
        }
    }

    public class UpdatePharmaceuticalCategoryRequestHandler :
        IRequestHandler<UpdatePharmaceuticalCategoryRequest, ResponseModelView>
    {
        private readonly IPharmaceuticalCategoryRepository _repository;
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        public UpdatePharmaceuticalCategoryRequestHandler(
            IPharmaceuticalCategoryRepository repository,
            IVendorRepository vendorRepository,
            IMapper mapper
            )
        {
            _repository = repository;
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdatePharmaceuticalCategoryRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkVendorExist = _vendorRepository.FindBy(x => x.Id ==
                request.PharmaceuticalCategoryDto!.VendorId
            ).FirstOrDefault();

            if (checkVendorExist == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Vendor not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var category = _repository.FindBy(x => 
                x.Id == request.Id
            ).FirstOrDefault();

            if (category == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Pharmaceutical Category not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            category.Name = request.PharmaceuticalCategoryDto!.Name;
            category.Description = request.PharmaceuticalCategoryDto!.Description;
            category.DateModified = DateTime.Now;
            category.Active = request.PharmaceuticalCategoryDto?.Active ?? category.Active;
            category.VendorId = request.PharmaceuticalCategoryDto!.VendorId;

            _repository.Update(category);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<PharmaceuticalCategory, PharmaceuticalCategoryView>(category);

            return Task.FromResult(result);
        }
    }

    public class CheckPharmaceuticalCategoryExistRequestHandler :
        IRequestHandler<CheckPharmaceuticalCategoryExistRequest, ResponseModelView>
    {
        private readonly IPharmaceuticalCategoryRepository _repository;
        private readonly IMapper _mapper;

        public CheckPharmaceuticalCategoryExistRequestHandler(
            IPharmaceuticalCategoryRepository repository,
            IMapper mapper
            )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckPharmaceuticalCategoryExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var categoryExist = _repository.FindBy(x => x.Id == request.CategoryId).AsNoTracking().Any();

            if (!categoryExist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Pharmaceutical Category not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = categoryExist;

            return Task.FromResult(result);
        }
    }

    public class CheckPharmaceuticalCategoryNameExistRequestHandler :
        IRequestHandler<CheckPharmaceuticalCategoryNameExistRequest, ResponseModelView>
    {
        private readonly IPharmaceuticalCategoryRepository _repository;
        private readonly IMapper _mapper;

        public CheckPharmaceuticalCategoryNameExistRequestHandler(
            IPharmaceuticalCategoryRepository repository,
            IMapper mapper
            )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckPharmaceuticalCategoryNameExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var category = _repository.FindBy(x => 
                x.Name!.Equals(request.Name) &&
                x.VendorId == request.VendorId
            );

            if(request.CategoryId > 0)
            {
                category = category.Where(x => x.Id != request.CategoryId);
            }

            var exist = category.AsNoTracking().Any();

            if (exist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Pharmaceutical Category already exist!";
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

    public class GetSinglePharmaceuticalCategoryRequestHandler :
        IRequestHandler<GetSinglePharmaceuticalCategoryRequest, ResponseModelView>
    {
        private readonly IPharmaceuticalCategoryRepository _repository;
        private readonly IMapper _mapper;

        public GetSinglePharmaceuticalCategoryRequestHandler(
            IPharmaceuticalCategoryRepository repository,
            IMapper mapper
            )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSinglePharmaceuticalCategoryRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var category = _repository.FindBy(x => x.Id == request.Id)
                .Include(x => x.Vendor)
                .AsNoTracking().FirstOrDefault();

            if (category == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Pharmaceutical Category not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<PharmaceuticalCategory, PharmaceuticalCategoryView>(category);

            return Task.FromResult(result);
        }
    }

    public class GetAllPharmaceuticalCategoryRequestHandler :
        IRequestHandler<GetAllPharmaceuticalCategoryRequest, ResponseModelView>
    {
        private readonly IPharmaceuticalCategoryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPharmaceuticalCategoryRequestHandler(
            IPharmaceuticalCategoryRepository repository,
            IMapper mapper
            )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllPharmaceuticalCategoryRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var categoriesList = _repository.GetAll()
                .Include(x => x.Vendor)
                .AsNoTracking().ToList();

            if (!categoriesList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Pharmaceutical Categories not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<PharmaceuticalCategory>, IEnumerable<PharmaceuticalCategoryView>>(categoriesList);

            return Task.FromResult(result);
        }
    }
}
