using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.Helpers;
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

    public class UpdateCountryStatusRequestHandler : IRequestHandler<UpdateCountryStatusRequest, ResponseModelView>
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;

        public UpdateCountryStatusRequestHandler(ICountryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateCountryStatusRequest request, CancellationToken cancellationToken)
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

            country.DateModified = DateTime.Now;
            country.Active = request.StatusChangeDto?.Active ?? country.Active;

            _repository.Update(country);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Country, CountryView>(country);

            return Task.FromResult(result);
        }
    }

    public class DeleteCountryRequestHandler : IRequestHandler<DeleteCountryRequest, ResponseModelView>
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;

        public DeleteCountryRequestHandler(ICountryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(DeleteCountryRequest request, CancellationToken cancellationToken)
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

            var res = _repository.Delete(country);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = res;

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
                country = country.Where(x => x.Id != request.CountryId);
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

    public class GetAllCountryListRequestHandler : IRequestHandler<GetAllCountryListRequest, ResponseModelView>
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCountryListRequestHandler(ICountryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllCountryListRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var countries = _repository.FindBy(x => x.Active == request.Active).AsNoTracking().ToList();

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

    public class GetAllCountryRequestHandler : IRequestHandler<GetAllCountryRequest, ResponsePaginationModelView>
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCountryRequestHandler(ICountryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponsePaginationModelView> Handle(GetAllCountryRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponsePaginationModelView();

            var predicate = PredicateBuilder.True<Country>();

            List<Country>? countryResult = new List<Country>();

            var countries = _repository.GetAll().AsNoTracking();

            if(!string.IsNullOrWhiteSpace(request.CountryFilterDto!.Name))
            {
                predicate = predicate.And(x => x.Name!.Contains(request.CountryFilterDto.Name))
                    .Or(x => x.Description!.Contains(request.CountryFilterDto.Name));
            }
            if(!string.IsNullOrWhiteSpace(request.CountryFilterDto.Code))
            {
                predicate = predicate.Or(x => x.Code!.Contains(request.CountryFilterDto.Code));
            }

            countries = countries.Where(predicate);

            if (request.CountryFilterDto.CurrentPage > 0 && request.CountryFilterDto.PageSize > 0)
            {
                var pagedResult = countries.GetPaged(request.CountryFilterDto.CurrentPage,
                    request.CountryFilterDto.PageSize);

                countryResult = pagedResult.Results as List<Country>;
                result.CurrentPage = pagedResult.CurrentPage;
                result.PageSize = pagedResult.PageSize;
                result.TotalRecords = pagedResult.RowCount;
                result.TotalPages = pagedResult.PageCount;

            }
            else
            {
                countryResult = countries.ToList();
            }

            if (!countryResult!.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Countries not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Country>, IEnumerable<CountryView>>(countryResult!);

            return Task.FromResult(result);
        }
    }
}
