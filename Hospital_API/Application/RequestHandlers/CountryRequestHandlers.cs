using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            result.IsSuccessful = true;
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
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            country.Name = request.CountryDto?.Name;
            country.Code = request.CountryDto?.Code ?? country.Code;
            country.Description = request.CountryDto?.Description ?? country.Description;
            country.DateModified = DateTime.Now;
            country.Active = request.CountryDto?.Active ?? country.Active;

            _repository.Update(country);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Country, CountryView>(country);

            return Task.FromResult(result);
        }
    }

    public class CheckCountryExistRequestHandler : IRequestHandler<CheckCountryExistRequest, ResponseModelView>
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;

        public CheckCountryExistRequestHandler(ICountryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckCountryExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var countryExist = _repository.FindBy(x => x.Id == request.CountryId).AsNoTracking().Any();

            if (!countryExist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Country not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = countryExist;

            return Task.FromResult(result);
        }
    }

    public class CheckCountryNameExistRequestHandler : IRequestHandler<CheckCountryNameExistRequest, ResponseModelView>
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;

        public CheckCountryNameExistRequestHandler(ICountryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckCountryNameExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var country = _repository.FindBy(x => x.Name!.Equals(request.Name));

            if(request.CountryId > 0)
            {
                country = country.Where(x => x.Id == request.CountryId);
            }

            var exist = country.AsNoTracking().Any();

            if (exist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Country already exist!";
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

            var country = _repository.FindBy(x => x.Id == request.Id).AsNoTracking().FirstOrDefault();

            if (country == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Country not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
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

            var countries = _repository.GetAll().AsNoTracking().ToList();

            if (!countries.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Countries not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Country>, IEnumerable<CountryView>>(countries);

            return Task.FromResult(result);
        }
    }
}
