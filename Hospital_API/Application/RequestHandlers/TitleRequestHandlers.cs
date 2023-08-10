using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddTitleRequestHandler : IRequestHandler<AddTitleRequest, ResponseModelView>
    {
        private readonly ITitleRepository _repository;
        private readonly IMapper _mapper;

        public AddTitleRequestHandler(ITitleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddTitleRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var currentDate = DateTime.Now;

            var title = new Title()
            {
                Name = request.TitleDto!.Name,
                Description = request.TitleDto!.Description,
                Active = request.TitleDto!.Active,
                DateCreated = currentDate,
                DateModified = currentDate
            };

            _repository.Add(title);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Title, TitleView>(title);

            return Task.FromResult(result);
        }
    }

    public class UpdateTitleRequestHandler : IRequestHandler<UpdateTitleRequest, ResponseModelView>
    {
        private readonly ITitleRepository _repository;
        private readonly IMapper _mapper;

        public UpdateTitleRequestHandler(ITitleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateTitleRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var title = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (title == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Title not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            title.Name = request.TitleDto!.Name;
            title.Description = request.TitleDto!.Description;
            title.Active = request.TitleDto?.Active ?? title.Active;
            title.DateModified = DateTime.Now;

            _repository.Update(title);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Title, TitleView>(title);

            return Task.FromResult(result);
        }
    }

    public class UpdateTitleStatusRequestHandler : IRequestHandler<UpdateTitleStatusRequest, ResponseModelView>
    {
        private readonly ITitleRepository _repository;
        private readonly IMapper _mapper;

        public UpdateTitleStatusRequestHandler(ITitleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateTitleStatusRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var title = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (title == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Title not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            title.DateModified = DateTime.Now;
            title.Active = request.StatusChangeDto?.Active ?? title.Active;

            _repository.Update(title);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Title, TitleView>(title);

            return Task.FromResult(result);
        }
    }

    public class DeleteTitleRequestHandler : IRequestHandler<DeleteTitleRequest, ResponseModelView>
    {
        private readonly ITitleRepository _repository;
        private readonly IMapper _mapper;

        public DeleteTitleRequestHandler(ITitleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(DeleteTitleRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var title = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (title == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Title not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var res = _repository.Delete(title);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = res;

            return Task.FromResult(result);
        }
    }

    public class CheckTitleExistRequestHandler : IRequestHandler<CheckTitleExistRequest, ResponseModelView>
    {
        private readonly ITitleRepository _repository;
        private readonly IMapper _mapper;

        public CheckTitleExistRequestHandler(ITitleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckTitleExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var titleExist = _repository.FindBy(x => x.Id == request.TitleId).AsNoTracking().Any();

            if (!titleExist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Title not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = titleExist;

            return Task.FromResult(result);
        }
    }

    public class CheckTitleNameExistRequestHandler : IRequestHandler<CheckTitleNameExistRequest, ResponseModelView>
    {
        private readonly ITitleRepository _repository;
        private readonly IMapper _mapper;

        public CheckTitleNameExistRequestHandler(ITitleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckTitleNameExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var title = _repository.FindBy(x => 
                x.Name!.Equals(request.Name)
            );

            if(request.TitleId > 0)
            {
                title = title.Where(x => x.Id != request.TitleId);
            }

            var exist = title.AsNoTracking().Any();

            if (exist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Title already exist!";
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

    public class GetSingleTitleRequestHandler : IRequestHandler<GetSingleTitleRequest, ResponseModelView>
    {
        private readonly ITitleRepository _repository;
        private readonly IMapper _mapper;

        public GetSingleTitleRequestHandler(ITitleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSingleTitleRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var title = _repository.FindBy(x => x.Id == request.Id).AsNoTracking().FirstOrDefault();

            if (title == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Title not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Title, TitleView>(title);

            return Task.FromResult(result);
        }
    }

    public class GetAllTitleListRequestHandler : IRequestHandler<GetAllTitleListRequest, ResponseModelView>
    {
        private readonly ITitleRepository _repository;
        private readonly IMapper _mapper;

        public GetAllTitleListRequestHandler(ITitleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllTitleListRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var titles = _repository.FindBy(x => x.Active == request.Active).AsNoTracking().ToList();

            if (!titles.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Titles not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Title>, IEnumerable<TitleView>>(titles);

            return Task.FromResult(result);
        }
    }

    public class GetAllTitleRequestHandler : IRequestHandler<GetAllTitleRequest, ResponseModelView>
    {
        private readonly ITitleRepository _repository;
        private readonly IMapper _mapper;

        public GetAllTitleRequestHandler(ITitleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllTitleRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var titlesList = _repository.GetAll().AsNoTracking().ToList();

            if (!titlesList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Titles not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Title>, IEnumerable<TitleView>>(titlesList);

            return Task.FromResult(result);
        }
    }
}
