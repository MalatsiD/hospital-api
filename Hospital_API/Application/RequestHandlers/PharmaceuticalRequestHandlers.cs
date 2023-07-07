using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddPharmaceuticalRequestHandler : IRequestHandler<AddPharmaceuticalRequest, ResponseModelView>
    {
        private readonly IPharmaceuticalRepository _repository;
        private readonly IPharmaceuticalCategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public AddPharmaceuticalRequestHandler(
            IPharmaceuticalRepository repository, 
            IPharmaceuticalCategoryRepository categoryRepository,
            IMapper mapper
            )
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddPharmaceuticalRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var currentDate = DateTime.Now;

            Pharmaceutical pharmaceutical = new Pharmaceutical()
            {
                Name = request.PharmaceuticalDto?.Name,
                Code = request.PharmaceuticalDto!.Code,
                Description = request.PharmaceuticalDto?.Description,
                Quantity = request.PharmaceuticalDto?.Quantity ?? 0,
                DateCreated = currentDate,
                DateModified = currentDate,
                Active = request.PharmaceuticalDto?.Active ?? true,
                PharmaceuticalCategoryId = request.PharmaceuticalDto!.PharmaceuticalCategoryId
            };

            _repository.Add(pharmaceutical);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Pharmaceutical, PharmaceuticalView>(pharmaceutical);

            return Task.FromResult( result );
        }
    }

    public class UpdatePharmaceuticalRequestHandler : IRequestHandler<UpdatePharmaceuticalRequest, ResponseModelView>
    {
        private readonly IPharmaceuticalRepository _repository;
        private readonly IPharmaceuticalCategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public UpdatePharmaceuticalRequestHandler(
            IPharmaceuticalRepository repository,
            IPharmaceuticalCategoryRepository categoryRepository,
            IMapper mapper
            )
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdatePharmaceuticalRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var pharmaceutical = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (pharmaceutical == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Pharmaceutical not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            pharmaceutical.Name = request.PharmaceuticalDto?.Name;
            pharmaceutical.Code = request.PharmaceuticalDto!.Code;
            pharmaceutical.Description = request.PharmaceuticalDto?.Description;
            pharmaceutical.Quantity = request.PharmaceuticalDto?.Quantity ?? pharmaceutical.Quantity;
            pharmaceutical.DateModified = DateTime.Now;
            pharmaceutical.Active = request.PharmaceuticalDto?.Active ?? pharmaceutical.Active;
            pharmaceutical.PharmaceuticalCategoryId = request.PharmaceuticalDto!.PharmaceuticalCategoryId;

            _repository.Update(pharmaceutical);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Pharmaceutical, PharmaceuticalView>(pharmaceutical);

            return Task.FromResult(result);
        }
    }

    public class CheckPharmaceuticalExistRequestHandler : IRequestHandler<CheckPharmaceuticalExistRequest, ResponseModelView>
    {
        private readonly IPharmaceuticalRepository _repository;
        private readonly IMapper _mapper;

        public CheckPharmaceuticalExistRequestHandler(IPharmaceuticalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckPharmaceuticalExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var pharmaceuticalExist = _repository.FindBy(x => x.Id == request.PharmaceuticalId).AsNoTracking().Any();

            if (!pharmaceuticalExist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Pharmaceutical not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = pharmaceuticalExist;

            return Task.FromResult(result);
        }
    }

    public class CheckPharmaceuticalNameExistRequestHandler : IRequestHandler<CheckPharmaceuticalNameExistRequest, ResponseModelView>
    {
        private readonly IPharmaceuticalRepository _repository;
        private readonly IMapper _mapper;

        public CheckPharmaceuticalNameExistRequestHandler(IPharmaceuticalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckPharmaceuticalNameExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var pharmaceutical = _repository.FindBy(x => 
                x.Name!.Equals(request.Name) &&
                x.PharmaceuticalCategoryId == request.CategoryId
            );

            if(request.PharmaceuticalId > 0)
            {
                pharmaceutical = pharmaceutical.Where(x => x.Id != request.PharmaceuticalId);
            }

            var exist = pharmaceutical.AsNoTracking().Any();

            if (exist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Pharmaceutical already exist!";
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

    public class GetSinglePharmaceuticalRequestHandler : IRequestHandler<GetSinglePharmaceuticalRequest, ResponseModelView>
    {
        private readonly IPharmaceuticalRepository _repository;
        private readonly IMapper _mapper;

        public GetSinglePharmaceuticalRequestHandler(IPharmaceuticalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSinglePharmaceuticalRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var pharmaceutical = _repository.FindBy(x => x.Id == request.Id)
                .Include(x => x.PharmaceuticalCategory)
                .AsNoTracking().FirstOrDefault();

            if (pharmaceutical == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Pharmaceutical not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Pharmaceutical, PharmaceuticalView>(pharmaceutical);

            return Task.FromResult(result);
        }
    }

    public class GetAllPharmaceuticalRequestHandler : IRequestHandler<GetAllPharmaceuticalRequest, ResponseModelView>
    {
        private readonly IPharmaceuticalRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPharmaceuticalRequestHandler(IPharmaceuticalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllPharmaceuticalRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var pharmaceuticalsList = _repository.GetAll()
                .Include(x => x.PharmaceuticalCategory)
                .AsNoTracking().ToList();

            if (!pharmaceuticalsList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Pharmaceuticals not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Pharmaceutical>, IEnumerable<PharmaceuticalView>>(pharmaceuticalsList);

            return Task.FromResult(result);
        }
    }
}
