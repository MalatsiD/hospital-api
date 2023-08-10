using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.ViewModels;
using Hospital_API.ViewModels.CityViews;
using MediatR;

namespace Hospital_API.Application.RequestHandlers
{
    public class CheckAilmentInAdmitAilmentExistRequestHandler : IRequestHandler<CheckAilmentInAdmitAilmentExistRequest, ResponseModelView>
    {
        private readonly IAdmitAilmentRepository _repository;

        public CheckAilmentInAdmitAilmentExistRequestHandler(IAdmitAilmentRepository repository)
        {
            _repository = repository;
        }

        public Task<ResponseModelView> Handle(CheckAilmentInAdmitAilmentExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkAdmitAilments = _repository.FindBy(x => x.AilmentId == request.AilmentId).Any();

            if(checkAdmitAilments)
            {
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = "Ailment cannot be deleted!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = checkAdmitAilments;

            return Task.FromResult(result);
        }
    }
}
