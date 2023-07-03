using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.ViewModels;
using MediatR;
using System.Net;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddCountryRequestHandler : IRequestHandler<AddCountryRequest, ResponseModelView>
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;

        public AddCountryRequestHandler(ICountryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddCountryRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkCountry = _repository.FindBy(x => x.Name!.Equals(request.CountryDto!.Name));

            if (checkCountry != null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Country already exists!";
                result.Response = false;

                return Task.FromResult(result);
            }

            DateTime currentDate = DateTime.Now;

            Country country = new Country()
            {
                Name = request.CountryDto?.Name,
                Code = request.CountryDto?.Code,
                Description = request.CountryDto?.Description,
                DateModified = currentDate,
                DateCreated = currentDate,
                Active = request.CountryDto?.Active ?? true
            };

            _repository.Add(country);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Country, CountryView>(country);

            return Task.FromResult(result);
        }
    }

    public class UpdateCountryRequestHandler : IRequestHandler<UpdateCountryRequest, ResponseModelView>
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;

        public UpdateCountryRequestHandler(ICountryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateCountryRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var country = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (country == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Country not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            country.Name = request.CountryDto?.Name;
            country.Code = request.CountryDto?.Code ?? country.Code;
            country.Description = request.CountryDto?.Description ?? country.Description;
            country.DateModified = DateTime.Now;
            country.Active = request.CountryDto?.Active ?? country.Active;

            result.Response = _repository.Update(country);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;

            return Task.FromResult(result);
        }
    }

    public class GetSingleCountryRequestHandler : IRequestHandler<GetSingleCountryRequest, ResponseModelView>
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;

        public GetSingleCountryRequestHandler(ICountryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSingleCountryRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var country = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (country == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Country not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Country, CountryView>(country);

            return Task.FromResult(result);
        }
    }

    public class GetAllCountryRequestHandler : IRequestHandler<GetAllCountryRequest, ResponseModelView>
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCountryRequestHandler(ICountryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllCountryRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var countries = _repository.GetAll().ToList();

            if (!countries.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Countries not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Country>, IEnumerable<CountryView>>(countries);

            return Task.FromResult(result);
        }
    }
}
