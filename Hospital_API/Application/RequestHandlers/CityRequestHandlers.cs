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

            var checkProvinceExist = _provinceRepository.FindBy(x => x.Id == request.CityDto!.ProvinceId).FirstOrDefault();

            if (checkProvinceExist == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Province does not exist!";
                result.Response = false;
                return Task.FromResult(result);
            }

            var checkCityExist = _repository.FindBy(x => 
                x.Name!.Equals(request.CityDto!.Name) &&
                x.ProvinceId == request.CityDto!.ProvinceId
            ).FirstOrDefault();

            if(checkCityExist != null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "City already exist!";
                result.Response = false;

                return Task.FromResult(result);
            }

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

            var checkProvinceExist = _provinceRepository.FindBy(x => x.Id == request.CityDto!.ProvinceId).FirstOrDefault();
            
            if (checkProvinceExist == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Province does not exist!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var city = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (city == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "City not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            city.Name = request.CityDto?.Name;
            city.Description = request.CityDto?.Description ?? city.Description;
            city.DateModified = DateTime.Now;
            city.ProvinceId = request.CityDto!.ProvinceId;
            city.Active = request.CityDto?.Active ?? city.Active;

            result.Response = _repository.Update(city);
            _repository.Commit();

            result.ErrorMessage = null;
            result.StatusCode = StatusCodes.Status200OK;

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
                .FirstOrDefault();

            if(city == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "City not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
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

            var cities = _repository.GetAll().Include(x => x.Province).ToList();

            if(!cities.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Cities not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.Response = _mapper.Map<IEnumerable<City>, IEnumerable<CityView>>(cities);
            result.ErrorMessage = null;
            result.StatusCode = StatusCodes.Status200OK;

            return Task.FromResult(result);
        }
    }
}
