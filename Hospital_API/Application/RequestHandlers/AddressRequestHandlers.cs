using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital_API.Application.RequestHandlers
{
    public class CheckCityInAddressExistRequestHandler : IRequestHandler<CheckCityInAddressExistRequest, ResponseModelView>
    {
        private readonly IAddressRepository _addressRepository;

        public CheckCityInAddressExistRequestHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public Task<ResponseModelView> Handle(CheckCityInAddressExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var cityExist = _addressRepository.FindBy(x => x.CityId == request.CityId).AsNoTracking().Any();

            if(cityExist)
            {
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = "City cannot be deleted!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = cityExist;

            return Task.FromResult(result);
        }
    }
}
