using Hospital_API.Entities;

namespace Hospital_API.Data.Abstract
{
    public interface ICountryRepository : IEntityBaseRepository<Country> { }
    public interface IProvinceRepository : IEntityBaseRepository<Province> { }
    public interface ICityRepository : IEntityBaseRepository<City> { }
    public interface IGenderRepository : IEntityBaseRepository<Gender> { }
    public interface ITitleRepository : IEntityBaseRepository<Title> { }
    public interface IAddressTypeRepository : IEntityBaseRepository<AddressType> { }
    public interface IAddressRepository : IEntityBaseRepository<Address> { }
    public interface IAilmentRepository : IEntityBaseRepository<Ailment> { }
    public interface IAdmitAilmentRepository : IEntityBaseRepository<AdmitAilment> { }
    public interface IHospitalRepository : IEntityBaseRepository<Hospital> { }
    public interface IHospitalAddressRepository : IEntityBaseRepository<HospitalAddress> { }
    public interface IDepartmentRepository : IEntityBaseRepository<Department> { }
    public interface IRoleRepository : IEntityBaseRepository<Role> { }
    public interface IVendorRepository : IEntityBaseRepository<Vendor> { }
    public interface IWardRepository : IEntityBaseRepository<Ward> { }
    public interface IPharmaceuticalCategoryRepository : IEntityBaseRepository<PharmaceuticalCategory> { }
    public interface IPharmaceuticalRepository : IEntityBaseRepository<Pharmaceutical> { }
    public interface IPersonRepository : IEntityBaseRepository<Person> { }
    public interface IPatientRepository : IEntityBaseRepository<Patient> { }
    public interface IEmployeeRepository : IEntityBaseRepository<Employee> { }
    public interface IEmployeeRoleRepository : IEntityBaseRepository<EmployeeRole> { }
    public interface IPatientAdmitRepository : IEntityBaseRepository<PatientAdmit> { }
}
