using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.Helpers;
using Hospital_API.ViewModels;
using Hospital_API.ViewModels.CityViews;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddCityRequestHandler : IRequestHandler<AddCityRequest, ResponseModelView>
    {
        private readonly ICityRepository _repository;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IMapper _mapper;

        public AddCityRequestHandler(ICityRepository repository, IProvinceRepository provinceRepository, IMapper mapper)
        {
            _repository = repository;
            _provinceRepository = provinceRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddCityRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            DateTime currentDate = DateTime.Now;

            City city = new City()
            {
                Name = request.CityDto?.Name,
                Description = request.CityDto?.Description,
                DateCreated = currentDate,
                DateModified = currentDate,
                Active = request.CityDto?.Active ?? true,
                ProvinceId = request.CityDto!.ProvinceId
            };

            _repository.Add(city);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<City, CityView>(city);

            return Task.FromResult(result);
        }
    }

    public class UpdateCityRequestHandler : IRequestHandler<UpdateCityRequest, ResponseModelView>
    {
        private readonly ICityRepository _repository;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IMapper _mapper;

        public UpdateCityRequestHandler(ICityRepository repository, IProvinceRepository provinceRepository, IMapper mapper)
        {
            _repository = repository;
            _provinceRepository = provinceRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateCityRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var city = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (city == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "City not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            city.Name = request.CityDto?.Name;
            city.Description = request.CityDto?.Description ?? city.Description;
            city.DateModified = DateTime.Now;
            city.ProvinceId = request.CityDto!.ProvinceId;
            city.Active = request.CityDto?.Active ?? city.Active;

            _repository.Update(city);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<City, CityView>(city);

            return Task.FromResult(result);
        }
    }

    public class UpdateCityStatusRequestHandler : IRequestHandler<UpdateCityStatusRequest, ResponseModelView>
    {
        private readonly ICityRepository _repository;
        private readonly IMapper _mapper;

        public UpdateCityStatusRequestHandler(ICityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateCityStatusRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var city = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (city == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "City not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            city.DateModified = DateTime.Now;
            city.Active = request.StatusChangeDto?.Active ?? city.Active;

            _repository.Update(city);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<City, CityView>(city);

            return Task.FromResult(result);
        }
    }

    public class DeleteCityRequestHandler : IRequestHandler<DeleteCityRequest, ResponseModelView>
    {
        private readonly ICityRepository _repository;
        private readonly IMapper _mapper;

        public DeleteCityRequestHandler(ICityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(DeleteCityRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var city = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (city == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "City not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var res = _repository.Delete(city);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = res;

            return Task.FromResult(result);
        }
    }

    public class CheckCityExistRequestHandler : IRequestHandler<CheckCityExistRequest, ResponseModelView>
    {
        private readonly ICityRepository _repository;
        private readonly IMapper _mapper;

        public CheckCityExistRequestHandler(ICityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckCityExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var cityExist = _repository.FindBy(x => x.Id == request.CityId).AsNoTracking().Any();

            if (!cityExist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "City not found!";
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

    public class CheckCityNameExistRequestHandler : IRequestHandler<CheckCityNameExistRequest, ResponseModelView>
    {
        private readonly ICityRepository _repository;
        private readonly IMapper _mapper;

        public CheckCityNameExistRequestHandler(ICityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckCityNameExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var city = _repository.FindBy(x => x.Name!.Equals(request.Name) && x.ProvinceId == request.ProvinceId);

            if(request.CityId > 0)
            {
                city = city.Where(x => x.Id == request.CityId);
            }

            var exist = city.AsNoTracking().Any();

            if (exist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "City already exist!";
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

    public class CheckProvinceInCityExistRequestHandler : IRequestHandler<CheckProvinceInCityExistRequest, ResponseModelView>
    {
        private readonly ICityRepository _repository;
        private readonly IMapper _mapper;

        public CheckProvinceInCityExistRequestHandler(ICityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckProvinceInCityExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var provinceExist = _repository.FindBy(x => x.ProvinceId == request.ProvinceId).AsNoTracking().Any();

            if (provinceExist)
            {
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = "Province cannot be deleted!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = provinceExist;

            return Task.FromResult(result);
        }
    }

    public class GetSingleCityRequestHandler : IRequestHandler<GetSingleCityRequest, ResponseModelView>
    {
        private readonly ICityRepository _repository;
        private readonly IMapper _mapper;

        public GetSingleCityRequestHandler(ICityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSingleCityRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var city = _repository.FindBy(x => x.Id == request.Id).Include(x => x.Province)
                .AsNoTracking().FirstOrDefault();

            if(city == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "City not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<City, CityView>(city!);

            return Task.FromResult(result);
        }
    }

    public class GetCityListRequestHandler : IRequestHandler<GetCityListRequest, ResponseModelView>
    {
        private readonly ICityRepository _repository;
        private readonly IMapper _mapper;

        public GetCityListRequestHandler(ICityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetCityListRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var cities = _repository.FindBy(x => 
                    x.ProvinceId == (request.ProvinceId > 0 ? request.ProvinceId : x.ProvinceId) &&
                    x.Active == request.Active
                )
                .AsNoTracking().ToList();

            if (!cities.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Cities not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<City>, IEnumerable<CityListView>>(cities);

            return Task.FromResult(result);
        }
    }

    public class GetAllCityRequestHandler : IRequestHandler<GetAllCityRequest, ResponsePaginationModelView>
    {
        private readonly ICityRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCityRequestHandler(ICityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponsePaginationModelView> Handle(GetAllCityRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponsePaginationModelView();

            var predicate = PredicateBuilder.True<City>();

            List<City>? cityResult = new List<City>();

            var cities = _repository.GetAll().Include(x => x.Province).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.CityFilterDto!.Name))
            {
                predicate = predicate.And(x => x.Name!.Contains(request.CityFilterDto.Name))
                    .Or(x => x.Description!.Contains(request.CityFilterDto.Name));
            }
            if (!string.IsNullOrWhiteSpace(request.CityFilterDto.ProvinceName))
            {
                predicate = predicate.Or(x => x.Province!.Name!.Contains(request.CityFilterDto.ProvinceName));
            }

            cities = cities.Where(predicate);

            if (request.CityFilterDto.CurrentPage > 0 && request.CityFilterDto.PageSize > 0)
            {
                var pagedResult = cities.GetPaged(request.CityFilterDto.CurrentPage,
                    request.CityFilterDto.PageSize);

                cityResult = pagedResult.Results as List<City>;
                result.CurrentPage = pagedResult.CurrentPage;
                result.PageSize = pagedResult.PageSize;
                result.TotalRecords = pagedResult.RowCount;
                result.TotalPages = pagedResult.PageCount;

            }
            else
            {
                cityResult = cities.ToList();
            }

            if (!cityResult!.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Cities not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<City>, IEnumerable<CityView>>(cityResult!);

            return Task.FromResult(result);
        }
    }
}
