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

    public class UpdateProvinceStatusRequestHandler : IRequestHandler<UpdateProvinceStatusRequest, ResponseModelView>
    {
        private readonly IProvinceRepository _repository;
        private readonly IMapper _mapper;

        public UpdateProvinceStatusRequestHandler(IProvinceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateProvinceStatusRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var province = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (province == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Province not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            province.DateModified = DateTime.Now;
            province.Active = request.StatusChangeDto?.Active ?? province.Active;

            _repository.Update(province);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Province, ProvinceView>(province);

            return Task.FromResult(result);
        }
    }

    public class DeleteProvinceRequestHandler : IRequestHandler<DeleteProvinceRequest, ResponseModelView>
    {
        private readonly IProvinceRepository _repository;
        private readonly IMapper _mapper;

        public DeleteProvinceRequestHandler(IProvinceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(DeleteProvinceRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var province = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (province == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Province not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            var res = _repository.Delete(province);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = res;

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

    public class GetAllProvinceListRequestHandler : IRequestHandler<GetAllProvinceListRequest, ResponseModelView>
    {
        private readonly IProvinceRepository _repository;
        private readonly IMapper _mapper;

        public GetAllProvinceListRequestHandler(IProvinceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllProvinceListRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var provinces = _repository.FindBy(x => x.Active == request.Active).AsNoTracking().ToList();

            if (!provinces.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Provinces not found!";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Province>, IEnumerable<ProvinceListView>>(provinces);

            return Task.FromResult(result);
        }
    }

    public class GetAllProvinceRequestHandler : IRequestHandler<GetAllProvinceRequest, ResponsePaginationModelView>
    {
        private readonly IProvinceRepository _repository;
        private readonly IMapper _mapper;

        public GetAllProvinceRequestHandler(IProvinceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponsePaginationModelView> Handle(GetAllProvinceRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponsePaginationModelView();

            var predicate = PredicateBuilder.True<Province>();

            List<Province>? provinceResult = new List<Province>();

            var provinces = _repository.GetAll().Include(x => x.Country).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.ProvinceFilterDto!.Name))
            {
                predicate = predicate.And(x => x.Name!.Contains(request.ProvinceFilterDto.Name))
                    .Or(x => x.Description!.Contains(request.ProvinceFilterDto.Name));
            }
            if (!string.IsNullOrWhiteSpace(request.ProvinceFilterDto.Code))
            {
                predicate = predicate.Or(x => x.Code!.Contains(request.ProvinceFilterDto.Code));
            }
            if (!string.IsNullOrWhiteSpace(request.ProvinceFilterDto.CountryName))
            {
                predicate = predicate.Or(x => x.Country!.Name!.Contains(request.ProvinceFilterDto.CountryName));
            }

            provinces = provinces.Where(predicate);

            if (request.ProvinceFilterDto.CurrentPage > 0 && request.ProvinceFilterDto.PageSize > 0)
            {
                var pagedResult = provinces.GetPaged(request.ProvinceFilterDto.CurrentPage,
                    request.ProvinceFilterDto.PageSize);

                provinceResult = pagedResult.Results as List<Province>;
                result.CurrentPage = pagedResult.CurrentPage;
                result.PageSize = pagedResult.PageSize;
                result.TotalRecords = pagedResult.RowCount;
                result.TotalPages = pagedResult.PageCount;

            }
            else
            {
                provinceResult = provinces.ToList();
            }

            if (!provinceResult!.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Provinces not found";
                result.IsSuccessful = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.IsSuccessful = true;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Province>, IEnumerable<ProvinceView>>(provinceResult!);

            return Task.FromResult(result);
        }
    }
}
