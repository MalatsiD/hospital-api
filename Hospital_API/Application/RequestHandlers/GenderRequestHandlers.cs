using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

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
            result.IsSuccessful = true;
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
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            gender.Name = request.GenderDto?.Name;
            gender.Description = request.GenderDto?.Description;
            gender.Active = request.GenderDto?.Active ?? gender.Active;
            gender.DateModified = DateTime.Now;

            _repository.Update(gender);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Gender, GenderView>(gender);

            return Task.FromResult(result);
        }
    }

    public class UpdateGenderStatusRequestHandler : IRequestHandler<UpdateGenderStatusRequest, ResponseModelView>
    {
        private readonly IGenderRepository _repository;
        private readonly IMapper _mapper;

        public UpdateGenderStatusRequestHandler(IGenderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateGenderStatusRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var gender = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (gender == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Gender not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            gender.DateModified = DateTime.Now;
            gender.Active = request.StatusChangeDto?.Active ?? gender.Active;

            _repository.Update(gender);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Gender, GenderView>(gender);

            return Task.FromResult(result);
        }
    }

    public class DeleteGenderRequestHandler : IRequestHandler<DeleteGenderRequest, ResponseModelView>
    {
        private readonly IGenderRepository _repository;
        private readonly IMapper _mapper;

        public DeleteGenderRequestHandler(IGenderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(DeleteGenderRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var gender = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (gender == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Gender not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var res = _repository.Delete(gender);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = res;

            return Task.FromResult(result);
        }
    }

    public class CheckGenderExistRequestHandler : IRequestHandler<CheckGenderExistRequest, ResponseModelView>
    {
        private readonly IGenderRepository _repository;
        private readonly IMapper _mapper;

        public CheckGenderExistRequestHandler(IGenderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckGenderExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var genderExist = _repository.FindBy(x => x.Id == request.GenderId).AsNoTracking().Any();

            if (!genderExist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Gender not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = genderExist;

            return Task.FromResult(result);
        }
    }

    public class CheckGenderNameExistRequestHandler : IRequestHandler<CheckGenderNameExistRequest, ResponseModelView>
    {
        private readonly IGenderRepository _repository;
        private readonly IMapper _mapper;

        public CheckGenderNameExistRequestHandler(IGenderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckGenderNameExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var gender = _repository.FindBy(x => 
                x.Name!.Equals(request.Name)
            );

            if(request.GenderId > 0)
            {
                gender = gender.Where(x => x.Id != request.GenderId);
            }

            var exist = gender.AsNoTracking().Any();

            if (exist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Gender already exist!";
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

            var gender = _repository.FindBy(x => x.Id == request.Id).AsNoTracking().FirstOrDefault();

            if (gender == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Gender not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Gender, GenderView>(gender);

            return Task.FromResult(result);
        }
    }

    public class GetAllGenderListRequestHandler : IRequestHandler<GetAllGenderListRequest, ResponseModelView>
    {
        private readonly IGenderRepository _repository;
        private readonly IMapper _mapper;

        public GetAllGenderListRequestHandler(IGenderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllGenderListRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var genders = _repository.FindBy(x => x.Active == request.Active).AsNoTracking().ToList();

            if (!genders.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Genders not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Gender>, IEnumerable<GenderView>>(genders);

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

            var genderList = _repository.GetAll().AsNoTracking().ToList();

            if (!genderList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Genders not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Gender>, IEnumerable<GenderView>>(genderList);

            return Task.FromResult(result);
        }
    }
}
