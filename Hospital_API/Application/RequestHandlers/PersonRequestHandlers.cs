using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.RequestHandlers
{
    public class CheckGenderInPersonExistRequestHandler : IRequestHandler<CheckGenderInPersonExistRequest, ResponseModelView>
    {
        private readonly IPersonRepository _repository;

        public CheckGenderInPersonExistRequestHandler(IPersonRepository repository)
        {
            _repository = repository;
        }

        public Task<ResponseModelView> Handle(CheckGenderInPersonExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkGender = _repository.FindBy(x => x.GenderId == request.GenderId).Any();

            if(checkGender)
            {
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = "Gender cannot be deleted!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.IsSuccessful = true;
            result.Response = checkGender;

            return Task.FromResult(result);
        }
    }

    public class CheckTitleInPersonExistRequestHandler : IRequestHandler<CheckTitleInPersonExistRequest, ResponseModelView>
    {
        private readonly IPersonRepository _repository;

        public CheckTitleInPersonExistRequestHandler(IPersonRepository repository)
        {
            _repository = repository;
        }

        public Task<ResponseModelView> Handle(CheckTitleInPersonExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkTitle = _repository.FindBy(x => x.TitleId == request.TitleId).Any();

            if (checkTitle)
            {
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = "Title cannot be deleted!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.IsSuccessful = true;
            result.Response = checkTitle;

            return Task.FromResult(result);
        }
    }
}
