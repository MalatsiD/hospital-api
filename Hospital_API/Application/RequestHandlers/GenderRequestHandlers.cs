using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddGenderRequestHandler : IRequestHandler<AddGenderRequest, ResponseModelView>
    {
        private readonly IGenderRepository _repository;
        private readonly IMapper _mapper;

        public AddGenderRequestHandler(IGenderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddGenderRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkGenderExist = _repository.FindBy(x => x.Name!.Equals(request.GenderDto!.Name)).FirstOrDefault();

            if (checkGenderExist != null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Gender already exist!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            var gender = new Gender()
            {
                Name = request.GenderDto?.Name,
                Description = request.GenderDto?.Description,
                Active = request.GenderDto?.Active ?? true,
                DateCreated = currentDate,
                DateModified = currentDate
            };

            _repository.Add(gender);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Gender, GenderView>(gender);

            return Task.FromResult(result);
        }
    }

    public class UpdateGenderRequestHandler : IRequestHandler<UpdateGenderRequest, ResponseModelView>
    {
        private readonly IGenderRepository _repository;
        private readonly IMapper _mapper;

        public UpdateGenderRequestHandler(IGenderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateGenderRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var gender = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (gender == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Gender not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            gender.Name = request.GenderDto?.Name;
            gender.Description = request.GenderDto?.Description;
            gender.Active = request.GenderDto?.Active ?? gender.Active;
            gender.DateModified = DateTime.Now;

            result.Response = _repository.Update(gender);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;

            return Task.FromResult(result);
        }
    }

    public class GetSingleGenderRequestHandler : IRequestHandler<GetSingleGenderRequest, ResponseModelView>
    {
        private readonly IGenderRepository _repository;
        private readonly IMapper _mapper;

        public GetSingleGenderRequestHandler(IGenderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSingleGenderRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var gender = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (gender == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Gender not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Gender, GenderView>(gender);

            return Task.FromResult(result);
        }
    }

    public class GetAllGenderRequestHandler : IRequestHandler<GetAllGenderRequest, ResponseModelView>
    {
        private readonly IGenderRepository _repository;
        private readonly IMapper _mapper;

        public GetAllGenderRequestHandler(IGenderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllGenderRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var genderList = _repository.GetAll().ToList();

            if (!genderList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Genders not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Gender>, IEnumerable<GenderView>>(genderList);

            return Task.FromResult(result);
        }
    }
}
