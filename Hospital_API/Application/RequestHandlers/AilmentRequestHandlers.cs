using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.Helpers;
using Hospital_API.Helpers.Pagination;
using Hospital_API.ViewModels;
using Hospital_API.ViewModels.CountryViews;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddAilmentRequestHandler : IRequestHandler<AddAilmentRequest, ResponseModelView>
    {
        private readonly IAilmentRepository _repository;
        private readonly IMapper _mapper;

        public AddAilmentRequestHandler(IAilmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddAilmentRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var currentDate = DateTime.Now;

            Ailment ailment = new Ailment()
            {
                Name = request.AilmentDto!.Name,
                Description = request.AilmentDto.Description,
                Active = request.AilmentDto.Active,
                DateCreated = currentDate,
                DateModified = currentDate
            };

            _repository.Add(ailment);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Ailment, AilmentView>(ailment);

            return Task.FromResult(result);
        }
    }

    public class UpdateAilmentRequestHandler : IRequestHandler<UpdateAilmentRequest, ResponseModelView>
    {
        private readonly IAilmentRepository _repository;
        private readonly IMapper _mapper;

        public UpdateAilmentRequestHandler(IAilmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateAilmentRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var ailment = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (ailment == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Ailment not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            ailment.Name = request.AilmentDto!.Name;
            ailment.Description = request.AilmentDto.Description;
            ailment.Active = request.AilmentDto?.Active ?? ailment.Active;
            ailment.DateModified = DateTime.Now;

            _repository.Update(ailment);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Ailment, AilmentView>(ailment);

            return Task.FromResult(result);
        }
    }

    public class UpdateAilmentStatusRequestHandler : IRequestHandler<UpdateAilmentStatusRequest, ResponseModelView>
    {
        private readonly IAilmentRepository _repository;
        private readonly IMapper _mapper;

        public UpdateAilmentStatusRequestHandler(IAilmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateAilmentStatusRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var ailment = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (ailment == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Ailment not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            ailment.DateModified = DateTime.Now;
            ailment.Active = request.StatusChangeDto?.Active ?? ailment.Active;

            _repository.Update(ailment);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Ailment, AilmentView>(ailment);

            return Task.FromResult(result);
        }
    }

    public class DeleteAilmentRequestHandler : IRequestHandler<DeleteAilmentRequest, ResponseModelView>
    {
        private readonly IAilmentRepository _repository;
        private readonly IMapper _mapper;

        public DeleteAilmentRequestHandler(IAilmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(DeleteAilmentRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var ailment = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (ailment == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Ailment not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var res = _repository.Delete(ailment);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = res;

            return Task.FromResult(result);
        }
    }

    public class CheckAilmentExistRequestHandler : IRequestHandler<CheckAilmentExistRequest, ResponseModelView>
    {
        private readonly IAilmentRepository _repository;
        private readonly IMapper _mapper;

        public CheckAilmentExistRequestHandler(IAilmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckAilmentExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var ailmentExist = _repository.FindBy(x => x.Id == request.AilmentId).AsNoTracking().Any();

            if (!ailmentExist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Ailment not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = ailmentExist;

            return Task.FromResult(result);
        }
    }

    public class CheckAilmentNameExistRequestHandler : IRequestHandler<CheckAilmentNameExistRequest, ResponseModelView>
    {
        private readonly IAilmentRepository _repository;
        private readonly IMapper _mapper;

        public CheckAilmentNameExistRequestHandler(IAilmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckAilmentNameExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var ailment = _repository.FindBy(x => x.Name!.Equals(request.Name));

            if (request.AilmentId > 0)
            {
                ailment = ailment.Where(x => x.Id != request.AilmentId);
            }

            var exist = ailment.AsNoTracking().Any();

            if (exist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Ailment already exist!";
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

    public class GetSingleAilmentRequestHandler : IRequestHandler<GetSingleAilmentRequest, ResponseModelView>
    {
        private readonly IAilmentRepository _repository;
        private readonly IMapper _mapper;

        public GetSingleAilmentRequestHandler(IAilmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSingleAilmentRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var ailment = _repository.FindBy(x => x.Id == request.Id).AsNoTracking().FirstOrDefault();

            if (ailment == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Ailment not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Ailment, AilmentView>(ailment);

            return Task.FromResult(result);
        }
    }

    public class GetAllAilmentListRequestHandler : IRequestHandler<GetAllAilmentListRequest, ResponseModelView>
    {
        private readonly IAilmentRepository _repository;
        private readonly IMapper _mapper;

        public GetAllAilmentListRequestHandler(IAilmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllAilmentListRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var ailments = _repository.FindBy(x => x.Active == request.Active).AsNoTracking().ToList();

            if (!ailments.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Ailments not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Ailment>, IEnumerable<AilmentView>>(ailments);

            return Task.FromResult(result);
        }
    }

    public class GetAllAilmentRequestHandler : IRequestHandler<GetAllAilmentRequest, ResponsePaginationModelView>
    {
        private readonly IAilmentRepository _repository;
        private readonly IMapper _mapper;

        public GetAllAilmentRequestHandler(IAilmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponsePaginationModelView> Handle(GetAllAilmentRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponsePaginationModelView();

            var predicate = PredicateBuilder.True<Ailment>();

            List<Ailment>? ailmentResult = new List<Ailment>();

            var ailments = _repository.GetAll().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.ailmentFilterDto!.Name))
            {
                predicate = predicate.And(x => x.Name!.Contains(request.ailmentFilterDto.Name))
                    .Or(x => x.Description!.Contains(request.ailmentFilterDto.Name));
            }

            ailments = ailments.Where(predicate);

            if (request.ailmentFilterDto.CurrentPage > 0 && request.ailmentFilterDto.PageSize > 0)
            {
                var pagedResult = ailments.GetPaged(request.ailmentFilterDto.CurrentPage,
                    request.ailmentFilterDto.PageSize);

                ailmentResult = pagedResult.Results as List<Ailment>;
                result.CurrentPage = pagedResult.CurrentPage;
                result.PageSize = pagedResult.PageSize;
                result.TotalRecords = pagedResult.RowCount;
                result.TotalPages = pagedResult.PageCount;

            }
            else
            {
                ailmentResult = ailments.ToList();
            }

            if (!ailmentResult!.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Ailments not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Ailment>, IEnumerable<AilmentView>>(ailmentResult!);

            return Task.FromResult(result);
        }
    }
}
