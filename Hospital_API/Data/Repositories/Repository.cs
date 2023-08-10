using Hospital_API.Data.Abstract;
using Hospital_API.Entities;

namespace Hospital_API.Data.Repositories
{
    public class CountryRepository : EntityBaseRepository<Country>, ICountryRepository
    {
        public CountryRepository(MyDbContext context) : base(context) { }
    }

    public class ProvinceRepository : EntityBaseRepository<Province>, IProvinceRepository
    {
        public ProvinceRepository(MyDbContext context) : base(context) { }
    }

    public class CityRepository : EntityBaseRepository<City>, ICityRepository
    {
        public CityRepository(MyDbContext context) : base(context) { }
    }

    public class GenderRepository : EntityBaseRepository<Gender>, IGenderRepository
    {
        public GenderRepository(MyDbContext context) : base(context) { }
    }

    public class TitleRepository : EntityBaseRepository<Title>, ITitleRepository
    {
        public TitleRepository(MyDbContext context) : base(context) { }
    }

    public class AddressTypeRepository : EntityBaseRepository<AddressType>, IAddressTypeRepository
    {
        public AddressTypeRepository(MyDbContext context) : base(context) { }
    }

    public class AddressRepository : EntityBaseRepository<Address>, IAddressRepository
    {
        public AddressRepository(MyDbContext context) : base(context) { }
    }

    public class AilmentRepository : EntityBaseRepository<Ailment>, IAilmentRepository
    {
        public AilmentRepository(MyDbContext context) : base(context) { }
    }

    public class AdmitAilmentRepository : EntityBaseRepository<AdmitAilment>, IAdmitAilmentRepository
    {
        public AdmitAilmentRepository(MyDbContext context) : base(context) { }
    }

    public class HospitalRepository : EntityBaseRepository<Hospital>, IHospitalRepository
    {
        public HospitalRepository(MyDbContext context) : base(context) { }
    }

    public class HospitalAddressRepository : EntityBaseRepository<HospitalAddress>, IHospitalAddressRepository
    {
        public HospitalAddressRepository(MyDbContext context) : base(context) { }
    }

    public class DepartmentRepository : EntityBaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(MyDbContext context) : base(context) { }
    }

    public class RoleRepository : EntityBaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(MyDbContext context) : base(context) { }
    }

    public class VendorRepository : EntityBaseRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(MyDbContext context) : base(context) { }
    }

    public class WardRepository : EntityBaseRepository<Ward>, IWardRepository
    {
        public WardRepository(MyDbContext context) : base(context) { }
    }

    public class PharmaceuticalCategoryRepository : EntityBaseRepository<PharmaceuticalCategory>, IPharmaceuticalCategoryRepository
    {
        public PharmaceuticalCategoryRepository(MyDbContext context) : base(context) { }
    }

    public class PharmaceuticalRepository : EntityBaseRepository<Pharmaceutical>, IPharmaceuticalRepository
    {
        public PharmaceuticalRepository(MyDbContext context) : base(context) { }
    }

    public class PersonRepository : EntityBaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(MyDbContext context) : base(context) { }
    }

    public class PatientRepository : EntityBaseRepository<Patient>, IPatientRepository
    {
        public PatientRepository(MyDbContext context) : base(context) { }
    }

    public class EmployeeRepository : EntityBaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(MyDbContext context) : base(context) { }
    }

    public class EmployeeRoleRepository : EntityBaseRepository<EmployeeRole>, IEmployeeRoleRepository
    {
        public EmployeeRoleRepository(MyDbContext context) : base(context) { }
    }

    public class PatientAdmitRepository : EntityBaseRepository<PatientAdmit>, IPatientAdmitRepository
    {
        public PatientAdmitRepository(MyDbContext context) : base(context) { }
    }
}
