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

    public class GetAllCityRequestHandler : IRequestHandler<GetAllCityRequest, ResponseModelView>
    {
        private readonly ICityRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCityRequestHandler(ICityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllCityRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var cities = _repository.GetAll().Include(x => x.Province).AsNoTracking().ToList();

            if(!cities.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Cities not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<City>, IEnumerable<CityView>>(cities);

            return Task.FromResult(result);
        }
    }
}
