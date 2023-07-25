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
    public class AddProvinceRequestHandler : IRequestHandler<AddProvinceRequest, ResponseModelView>
    {
        private readonly IProvinceRepository _repository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public AddProvinceRequestHandler(IProvinceRepository repository, ICountryRepository countryRepository, IMapper mapper)
        {
            _repository = repository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddProvinceRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            DateTime currentDate = DateTime.Now;

            Province province = new Province()
            {
                Name = request.ProvinceDto?.Name,
                Code = request.ProvinceDto?.Code,
                Description = request.ProvinceDto?.Description,
                CountryId = request.ProvinceDto!.CountryId,
                DateModified = currentDate,
                DateCreated = currentDate,
                Active = request.ProvinceDto.Active,
            };

            _repository.Add(province);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Province, ProvinceView>(province);

            return Task.FromResult(result);
        }
    }

    public class UpdateProvinceRequestHandler : IRequestHandler<UpdateProvinceRequest, ResponseModelView>
    {
        private readonly IProvinceRepository _repository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public UpdateProvinceRequestHandler(IProvinceRepository repository, ICountryRepository countryRepository, IMapper mapper)
        {
            _repository = repository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateProvinceRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var province = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if(province == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Province not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            province.Name = request.ProvinceDto?.Name;
            province.Code = request.ProvinceDto?.Code ?? province.Code;
            province.Description = request.ProvinceDto?.Description ?? province.Description;
            province.CountryId = request.ProvinceDto!.CountryId;
            province.DateModified = DateTime.Now;
            province.Active = request.ProvinceDto.Active;

            _repository.Update(province);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Province, ProvinceView>(province);

            return Task.FromResult(result);
        }
    }

    public class CheckProvinceExistRequestHandler : IRequestHandler<CheckProvinceExistRequest, ResponseModelView>
    {
        private readonly IProvinceRepository _repository;
        private readonly IMapper _mapper;

        public CheckProvinceExistRequestHandler(IProvinceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckProvinceExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var provinceExist = _repository.FindBy(x => x.Id == request.ProvinceId).AsNoTracking().Any();

            if (!provinceExist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Province not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = (int)HttpStatusCode.OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = provinceExist;

            return Task.FromResult(result);
        }
    }

    public class CheckProvinceNameExistRequestHandler : IRequestHandler<CheckProvinceNameExistRequest, ResponseModelView>
    {
        private readonly IProvinceRepository _repository;
        private readonly IMapper _mapper;

        public CheckProvinceNameExistRequestHandler(IProvinceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckProvinceNameExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var province = _repository.FindBy(x => 
                x.Name!.Equals(request.Name) &&
                x.CountryId == request.CountryId
            );

            if(request.ProvinceId > 0)
            {
                province = province.Where(x => x.Id != request.ProvinceId);
            }

            var exist = province.AsNoTracking().Any();

            if (exist)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Province already exist!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = (int)HttpStatusCode.OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = exist;

            return Task.FromResult(result);
        }
    }

    public class CheckCountryInProvinceExistRequestHandler : IRequestHandler<CheckCountryInProvinceExistRequest, ResponseModelView>
    {
        private readonly IProvinceRepository _repository;
        private readonly IMapper _mapper;

        public CheckCountryInProvinceExistRequestHandler(IProvinceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(CheckCountryInProvinceExistRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var countryExist = _repository.FindBy(x => x.CountryId == request.CountryId).AsNoTracking().Any();

            if (countryExist)
            {
                result.StatusCode = StatusCodes.Status200OK;
                result.ErrorMessage = "Country cannot be deleted!";
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

    public class GetSingleProvinceRequestHandler : IRequestHandler<GetSingleProvinceRequest, ResponseModelView>
    {
        private readonly IProvinceRepository _repository;
        private readonly IMapper _mapper;

        public GetSingleProvinceRequestHandler(IProvinceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSingleProvinceRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var province = _repository.FindBy(x => x.Id == request.Id)
                .Include(x => x.Country).AsNoTracking().FirstOrDefault();

            if (province == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Province not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result); 
            }

            result.StatusCode = (int)HttpStatusCode.OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Province, ProvinceView>(province);

            return Task.FromResult(result);
        }
    }

    public class GetAllProvinceRequestHandler : IRequestHandler<GetAllProvinceRequest, ResponseModelView>
    {
        private readonly IProvinceRepository _repository;
        private readonly IMapper _mapper;

        public GetAllProvinceRequestHandler(IProvinceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllProvinceRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var provinces = _repository.GetAll().Include(x => x.Country).AsNoTracking().ToList();

            if(!provinces.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Provinces not found";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Province>, IEnumerable<ProvinceView>>(provinces);

            return Task.FromResult(result);
        }
    }
}
