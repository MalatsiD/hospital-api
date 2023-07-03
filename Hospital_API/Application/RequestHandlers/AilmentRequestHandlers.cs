using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.ViewModels;
using MediatR;

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

            var checkAilmentExist = _repository.FindBy(x => x.Name!.Equals(request.AilmentDto!.Name)).FirstOrDefault();

            if (checkAilmentExist != null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Ailment already exist!";
                result.Response = false;

                return Task.FromResult(result);
            }

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
                result.Response = false;

                return Task.FromResult(result);
            }

            ailment.Name = request.AilmentDto!.Name;
            ailment.Description = request.AilmentDto.Description;
            ailment.Active = request.AilmentDto?.Active ?? ailment.Active;
            ailment.DateModified = DateTime.Now;

            result.Response = _repository.Update(ailment);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;

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

            var ailment = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (ailment == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Ailment not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Ailment, AilmentView>(ailment);

            return Task.FromResult(result);
        }
    }

    public class GetAllAilmentRequestHandler : IRequestHandler<GetAllAilmentRequest, ResponseModelView>
    {
        private readonly IAilmentRepository _repository;
        private readonly IMapper _mapper;

        public GetAllAilmentRequestHandler(IAilmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllAilmentRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var ailmentsList = _repository.GetAll().ToList();

            if (!ailmentsList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Ailments not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Ailment>, IEnumerable<AilmentView>>(ailmentsList);

            return Task.FromResult(result);
        }
    }
}
