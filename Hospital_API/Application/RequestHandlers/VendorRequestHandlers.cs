using AutoMapper;
using Hospital_API.Application.Requests;
using Hospital_API.Data.Abstract;
using Hospital_API.Entities;
using Hospital_API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital_API.Application.RequestHandlers
{
    public class AddVendorRequestHandler : IRequestHandler<AddVendorRequest, ResponseModelView>
    {
        private readonly IVendorRepository _repository;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IMapper _mapper;

        public AddVendorRequestHandler(IVendorRepository repository, IHospitalRepository hospitalRepository, IMapper mapper)
        {
            _repository = repository;
            _hospitalRepository = hospitalRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(AddVendorRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkHospitalExist = _hospitalRepository.FindBy(x => x.Id == request.VendorDto!.HospitalId).FirstOrDefault();

            if (checkHospitalExist == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Hospital not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var checkVendorExist = _repository.FindBy(x => 
                x.Name!.Equals(request.VendorDto!.Name) &&
                x.HospitalId == request.VendorDto!.HospitalId
            ).FirstOrDefault();

            if (checkVendorExist != null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Vendor already exist!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var currentDate = DateTime.Now;

            Vendor vendor = new Vendor()
            {
                Name = request.VendorDto!.Name,
                Code = request.VendorDto!.Code,
                Description = request.VendorDto!.Description,
                PhoneNumber = request.VendorDto!.PhoneNumber,
                Email = request.VendorDto!.Email,
                DateCreated = currentDate,
                DateModified = currentDate,
                Active = request.VendorDto?.Active ?? true,
                HospitalId = request.VendorDto!.HospitalId,
            };

            _repository.Add(vendor);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status201Created;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Vendor, VendorView>(vendor);

            return Task.FromResult(result);
        }
    }

    public class UpdateVendorRequestHandler : IRequestHandler<UpdateVendorRequest, ResponseModelView>
    {
        private readonly IVendorRepository _repository;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IMapper _mapper;

        public UpdateVendorRequestHandler(IVendorRepository repository, IHospitalRepository hospitalRepository, IMapper mapper)
        {
            _repository = repository;
            _hospitalRepository = hospitalRepository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(UpdateVendorRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var checkHospitalExist = _hospitalRepository.FindBy(x => x.Id == request.VendorDto!.HospitalId).FirstOrDefault();

            if (checkHospitalExist == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Hospital not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            var vendor = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (vendor == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.ErrorMessage = "Vendor not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            vendor.Name = request.VendorDto!.Name;
            vendor.Code = request.VendorDto!.Code;
            vendor.Description = request.VendorDto!.Description;
            vendor.PhoneNumber = request.VendorDto!.PhoneNumber;
            vendor.Email = request.VendorDto!.Email;
            vendor.DateModified = DateTime.Now;
            vendor.Active = request.VendorDto?.Active ?? vendor.Active;
            vendor.HospitalId = request.VendorDto!.HospitalId;

            _repository.Update(vendor);
            _repository.Commit();

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Vendor, VendorView>(vendor);

            return Task.FromResult(result);
        }
    }

    public class GetSingleVendorRequestHandler : IRequestHandler<GetSingleVendorRequest, ResponseModelView>
    {
        private readonly IVendorRepository _repository;
        private readonly IMapper _mapper;

        public GetSingleVendorRequestHandler(IVendorRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetSingleVendorRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var vendor = _repository.FindBy(x => x.Id == request.Id)
                .Include(x => x.Hospital)
                .FirstOrDefault();

            if (vendor == null)
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Vendor not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<Vendor, VendorView>(vendor);

            return Task.FromResult(result);
        }
    }

    public class GetAllVendorRequestHandler : IRequestHandler<GetAllVendorRequest, ResponseModelView>
    {
        private readonly IVendorRepository _repository;
        private readonly IMapper _mapper;

        public GetAllVendorRequestHandler(IVendorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseModelView> Handle(GetAllVendorRequest request, CancellationToken cancellationToken)
        {
            var result = new ResponseModelView();

            var vendorsList = _repository.GetAll()
                .Include(x => x.Hospital)
                .ToList();

            if (!vendorsList.Any())
            {
                result.StatusCode = StatusCodes.Status404NotFound;
                result.ErrorMessage = "Vendors not found!";
                result.Response = false;

                return Task.FromResult(result);
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.ErrorMessage = null;
            result.Response = _mapper.Map<IEnumerable<Vendor>, IEnumerable<VendorView>>(vendorsList);

            return Task.FromResult(result);
        }
    }
}
