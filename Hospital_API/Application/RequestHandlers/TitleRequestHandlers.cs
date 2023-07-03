using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.ViewModels;
using MediatR;

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

            var checkTitleExist = _repository.FindBy(x => x.Name!.Equals(request.TitleDto!.Name)).FirstOrDefault();

            if (checkTitleExist != null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Title already exist!";
                result.Response = false;

                return Task.FromResult(result);
            }

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
                result.Response = false;

                return Task.FromResult(result);
            }

            title.Name = request.TitleDto!.Name;
            title.Description = request.TitleDto!.Description;
            title.Active = request.TitleDto?.Active ?? title.Active;
            title.DateModified = DateTime.Now;

            result.Response = _repository.Update(title);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;

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

            var title = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (title == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Title not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Title, TitleView>(title);

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

            var titlesList = _repository.GetAll().ToList();

            if (!titlesList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Titles not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Title>, IEnumerable<TitleView>>(titlesList);

            return Task.FromResult(result);
        }
    }
}
