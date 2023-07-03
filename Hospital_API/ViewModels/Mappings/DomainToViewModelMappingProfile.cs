using AutoMapper;
using Hospital_API.Entities;
using Hospital_API.ModelViews.EmployeeViews;
using Hospital_API.ModelViews.PatientViews;
using Hospital_API.ViewModels.EmployeeViews;
using Hospital_API.ViewModels.PatientViews;

namespace Hospital_API.ViewModels.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Country, CountryView>();
            CreateMap<Province, ProvinceView>()
                .ForMember(pv => pv.CountryName, opt => opt.MapFrom(p => p.Country!.Name));
            CreateMap<City, CityView>()
                .ForMember(cw => cw.ProvinceName, opt => opt.MapFrom(c => c.Province!.Name));
            CreateMap<Gender, GenderView>();
            CreateMap<Title, TitleView>();
            CreateMap<AddressType, AddressTypeView>();
            CreateMap<Ailment, AilmentView>();

            CreateMap<Address, AddressView>()
                .ForMember(av => av.AddressTypeName, opt => opt.MapFrom(a => a.AddressType.Name))
                .ForMember(av => av.CityName, opt => opt.MapFrom(a => a.City.Name));

            CreateMap<HospitalAddress, HospitalAddressView>()
                .ForMember(hav => hav.AddressDetail, opt => opt.MapFrom(ha => ha.Address.AddressDetail))
                .ForMember(hav => hav.ZipCode, opt => opt.MapFrom(ha => ha.Address.ZipCode))
                .ForMember(hav => hav.CityId, opt => opt.MapFrom(ha => ha.Address.CityId))
                .ForMember(hav => hav.CityName, opt => opt.MapFrom(ha => ha.Address.City.Name))
                .ForMember(hav => hav.AddressTypeId, opt => opt.MapFrom(ha => ha.Address.AddressTypeId))
                .ForMember(hav => hav.AddressTypeName, opt => opt.MapFrom(ha => ha.Address.AddressType.Name))
                .ForMember(hav => hav.ProvinceId, opt => opt.MapFrom(ha => ha.Address.City.ProvinceId))
                .ForMember(hav => hav.ProvinceName, opt => opt.MapFrom(ha => ha.Address.City.Province!.Name))
                .ForMember(hav => hav.CountryId, opt => opt.MapFrom(ha => ha.Address.City.Province!.CountryId))
                .ForMember(hav => hav.CountryName, opt => opt.MapFrom(ha => ha.Address.City.Province!.Country!.Name));

            CreateMap<Hospital, HospitalView>()
                .ForMember(hv => hv.Addresses, opt => opt.MapFrom(h => h.Addresses));

            CreateMap<Department, DepartmentView>()
                .ForMember(dv => dv.HospitalName, opt => opt.MapFrom(d => d.Hospital.Name));

            CreateMap<Role, RoleView>();

            CreateMap<Vendor, VendorView>()
                .ForMember(vv => vv.HospitalName, opt => opt.MapFrom(v => v.Hospital!.Name));

            CreateMap<Ward, WardView>()
                .ForMember(wv => wv.DepartmentName, opt => opt.MapFrom(w => w.Department!.Name));

            CreateMap<PharmaceuticalCategory, PharmaceuticalCategoryView>()
                .ForMember(pcv => pcv.VenderName, opt => opt.MapFrom(pc => pc.Vendor!.Name));

            CreateMap<Pharmaceutical, PharmaceuticalView>()
                .ForMember(pv => pv.PharmaceuticalCategoryName, opt => opt.MapFrom(p => p.PharmaceuticalCategory!.Name));

            CreateMap<PersonAddress, PatientAddressView>()
                .ForMember(pav => pav.PatientId, opt => opt.MapFrom(pa => pa.Person.Patient!.Id))
                .ForMember(pav => pav.AddressDetail, opt => opt.MapFrom(pa => pa.Address.AddressDetail))
                .ForMember(pav => pav.ZipCode, opt => opt.MapFrom(pa => pa.Address.ZipCode))
                .ForMember(pav => pav.AddressTypeId, opt => opt.MapFrom(pa => pa.Address.AddressTypeId))
                .ForMember(pav => pav.AddressTypeName, opt => opt.MapFrom(pa => pa.Address.AddressType.Name))
                .ForMember(pav => pav.CityId, opt => opt.MapFrom(pa => pa.Address.CityId))
                .ForMember(pav => pav.CityName, opt => opt.MapFrom(pa => pa.Address.City.Name))
                .ForMember(pav => pav.ProvinceId, opt => opt.MapFrom(pa => pa.Address.City.ProvinceId))
                .ForMember(pav => pav.ProvinceName, opt => opt.MapFrom(pa => pa.Address.City.Province!.Name))
                .ForMember(pav => pav.CountryId, opt => opt.MapFrom(pa => pa.Address.City.Province!.CountryId))
                .ForMember(pav => pav.CountryName, opt => opt.MapFrom(pa => pa.Address.City.Province!.Country!.Name));

            CreateMap<Person, PatientView>()
                .ForMember(pv => pv.Id, opt => opt.MapFrom(p => p.Patient!.Id))
                .ForMember(pv => pv.PersonId, opt => opt.MapFrom(p => p.Id))
                .ForMember(pv => pv.FirstName, opt => opt.MapFrom(p => p.FirstName))
                .ForMember(pv => pv.MiddleName, opt => opt.MapFrom(p => p.MiddleName))
                .ForMember(pv => pv.LastName, opt => opt.MapFrom(p => p.LastName))
                .ForMember(pv => pv.FullName, opt => opt.MapFrom(p => string.Format("{0} {1}", p.FirstName, p.LastName)))
                .ForMember(pv => pv.DateOfBirth, opt => opt.MapFrom(p => p.DateOfBirth))
                .ForMember(pv => pv.Email, opt => opt.MapFrom(p => p.Email))
                .ForMember(pv => pv.PhoneNumber, opt => opt.MapFrom(p => p.PhoneNumber))
                .ForMember(pv => pv.TitleId, opt => opt.MapFrom(p => p.TitleId))
                .ForMember(pv => pv.TitleName, opt => opt.MapFrom(p => p!.Title!.Name))
                .ForMember(pv => pv.GenderId, opt => opt.MapFrom(p => p.GenderId))
                .ForMember(pv => pv.GenderName, opt => opt.MapFrom(p => p.Gender!.Name))
                .ForMember(pv => pv.PatientNumber, opt => opt.MapFrom(p => p.Patient!.PatientNumber))
                .ForMember(pv => pv.Addresses, opt => opt.MapFrom(p => p.Addresses))
                .ForMember(pv => pv.Active, opt => opt.MapFrom(p => p.Active));

            CreateMap<Patient, PatientView>()
                .ForMember(pv => pv.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(pv => pv.PersonId, opt => opt.MapFrom(p => p.PersonId))
                .ForMember(pv => pv.FirstName, opt => opt.MapFrom(p => p.Person!.FirstName))
                .ForMember(pv => pv.MiddleName, opt => opt.MapFrom(p => p.Person!.MiddleName))
                .ForMember(pv => pv.LastName, opt => opt.MapFrom(p => p.Person!.LastName))
                .ForMember(pv => pv.FullName, opt => opt.MapFrom(p => string.Format("{0} {1}", p.Person!.FirstName, p.Person!.LastName)))
                .ForMember(pv => pv.DateOfBirth, opt => opt.MapFrom(p => p.Person!.DateOfBirth))
                .ForMember(pv => pv.Email, opt => opt.MapFrom(p => p.Person!.Email))
                .ForMember(pv => pv.PhoneNumber, opt => opt.MapFrom(p => p.Person!.PhoneNumber))
                .ForMember(pv => pv.TitleId, opt => opt.MapFrom(p => p.Person!.TitleId))
                .ForMember(pv => pv.TitleName, opt => opt.MapFrom(p => p.Person!.Title!.Name))
                .ForMember(pv => pv.GenderId, opt => opt.MapFrom(p => p.Person!.GenderId))
                .ForMember(pv => pv.GenderName, opt => opt.MapFrom(p => p.Person!.Gender!.Name))
                .ForMember(pv => pv.PatientNumber, opt => opt.MapFrom(p => p.Person!.Patient!.PatientNumber))
                .ForMember(pv => pv.Addresses, opt => opt.MapFrom(p => p.Person!.Addresses))
                .ForMember(pv => pv.Active, opt => opt.MapFrom(p => p.Active));

            CreateMap<PersonAddress, EmployeeAddressView>()
                .ForMember(eav => eav.EmployeeId, opt => opt.MapFrom(ea => ea.Person!.Employee!.Id))
                .ForMember(eav => eav.AddressDetail, opt => opt.MapFrom(ea => ea.Address!.AddressDetail))
                .ForMember(eav => eav.ZipCode, opt => opt.MapFrom(ea => ea.Address!.ZipCode))
                .ForMember(eav => eav.AddressTypeId, opt => opt.MapFrom(ea => ea.Address!.AddressTypeId))
                .ForMember(eav => eav.AddressTypeName, opt => opt.MapFrom(ea => ea.Address!.AddressType.Name))
                .ForMember(eav => eav.CityId, opt => opt.MapFrom(ea => ea.Address!.CityId))
                .ForMember(eav => eav.CityName, opt => opt.MapFrom(ea => ea.Address!.City.Name))
                .ForMember(eav => eav.ProvinceId, opt => opt.MapFrom(ea => ea.Address!.City.ProvinceId))
                .ForMember(eav => eav.ProvinceName, opt => opt.MapFrom(ea => ea.Address!.City.Province!.Name))
                .ForMember(eav => eav.CountryId, opt => opt.MapFrom(ea => ea.Address!.City.Province!.CountryId))
                .ForMember(eav => eav.CountryName, opt => opt.MapFrom(ea => ea.Address!.City.Province!.Country!.Name));

            CreateMap<Person, EmployeeView>()
                .ForMember(ev => ev.Id, opt => opt.MapFrom(p => p.Patient!.Id))
                .ForMember(ev => ev.PersonId, opt => opt.MapFrom(p => p.Id))
                .ForMember(ev => ev.FirstName, opt => opt.MapFrom(p => p.FirstName))
                .ForMember(ev => ev.MiddleName, opt => opt.MapFrom(p => p.MiddleName))
                .ForMember(ev => ev.LastName, opt => opt.MapFrom(p => p.LastName))
                .ForMember(ev => ev.FullName, opt => opt.MapFrom(p => string.Format("{0} {1}", p.FirstName, p.LastName)))
                .ForMember(ev => ev.DateOfBirth, opt => opt.MapFrom(p => p.DateOfBirth))
                .ForMember(ev => ev.Email, opt => opt.MapFrom(p => p!.Email))
                .ForMember(ev => ev.PhoneNumber, opt => opt.MapFrom(p => p.PhoneNumber))
                .ForMember(ev => ev.TitleId, opt => opt.MapFrom(p => p.TitleId))
                .ForMember(ev => ev.TitleName, opt => opt.MapFrom(p => p.Title!.Name))
                .ForMember(ev => ev.GenderId, opt => opt.MapFrom(p => p.GenderId))
                .ForMember(ev => ev.GenderName, opt => opt.MapFrom(p => p.Gender!.Name))
                .ForMember(ev => ev.Addresses, opt => opt.MapFrom(p => p.Addresses))
                .ForMember(ev => ev.Active, opt => opt.MapFrom(p => p.Active));

            CreateMap<EmployeeManager, ManagerView>()
                .ForMember(mv => mv.EmployeeId, opt => opt.MapFrom(em => em.Employee!.Id))
                .ForMember(mv => mv.FirstName, opt => opt.MapFrom(em => em.Employee!.Person!.FirstName))
                .ForMember(mv => mv.MiddleName, opt => opt.MapFrom(em => em.Employee!.Person!.MiddleName))
                .ForMember(mv => mv.LastName, opt => opt.MapFrom(em => em.Employee!.Person!.LastName))
                .ForMember(mv => mv.FullName, opt => opt.MapFrom(em => string.Format("{0} {1}", em.Employee!.Person!.FirstName, em.Employee!.Person!.LastName)));

            CreateMap<Employee, EmployeeView>()
                    .ForMember(ev => ev.Id, opt => opt.MapFrom(e => e.Id))
                    .ForMember(ev => ev.PersonId, opt => opt.MapFrom(e => e.PersonId))
                    .ForMember(ev => ev.FirstName, opt => opt.MapFrom(e => e.Person!.FirstName))
                    .ForMember(ev => ev.MiddleName, opt => opt.MapFrom(e => e.Person!.MiddleName))
                    .ForMember(ev => ev.LastName, opt => opt.MapFrom(e => e.Person!.LastName))
                    .ForMember(ev => ev.FullName, opt => opt.MapFrom(e => string.Format("{0} {1}", e.Person!.FirstName, e.Person!.LastName)))
                    .ForMember(ev => ev.DateOfBirth, opt => opt.MapFrom(e => e.Person!.DateOfBirth))
                    .ForMember(ev => ev.Email, opt => opt.MapFrom(e => e.Person!.Email))
                    .ForMember(ev => ev.PhoneNumber, opt => opt.MapFrom(e => e.Person!.PhoneNumber))
                    .ForMember(ev => ev.TitleId, opt => opt.MapFrom(e => e.Person!.TitleId))
                    .ForMember(ev => ev.TitleName, opt => opt.MapFrom(e => e.Person!.Title!.Name))
                    .ForMember(ev => ev.GenderId, opt => opt.MapFrom(e => e.Person!.GenderId))
                    .ForMember(ev => ev.GenderName, opt => opt.MapFrom(e => e.Person!.Gender!.Name))
                    .ForMember(ev => ev.Addresses, opt => opt.MapFrom(e => e.Person!.Addresses))
                    .ForMember(ev => ev.Managers, opt => opt.MapFrom(e => e.Managers))
                    .ForMember(ev => ev.Active, opt => opt.MapFrom(e => e.Active));
        }
    }
}
