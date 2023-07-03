using Hospital_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hospital_API.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Ailment> Ailments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmploymentType> EmploymentTypes { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientAdmit> PatientAdmits { get; set; }
        public DbSet<Pharmaceutical> Pharmaceuticals { get; set; }
        public DbSet<PharmaceuticalCategory> PharmaceuticalCategories { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<DepartmentRole> DepartmentRoles { get; set; }
        public DbSet<DepartmentEmployee> departmentEmployees { get; set; }
        public DbSet<EmployeeManager> EmployeeManagers { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public DbSet<HospitalAddress> HospitalAddresses { get; set; }
        public DbSet<PersonAddress> PersonAddresses { get; set; }
        public DbSet<AdmitAilment> AdmitAilments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Employee)
                .WithOne(e => e.Person)
                .HasForeignKey<Employee>(e => e.PersonId);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Patient)
                .WithOne(pa => pa.Person)
                .HasForeignKey<Patient>(pa => pa.PersonId);

            modelBuilder.Entity<DepartmentRole>()
                .HasKey(dr => new { dr.DepartmentId, dr.RoleId });
            modelBuilder.Entity<DepartmentRole>()
                .HasOne(dr => dr.Department)
                .WithMany(dr => dr.Roles)
                .HasForeignKey(dr => dr.DepartmentId);
            modelBuilder.Entity<DepartmentRole>()
                .HasOne(dr => dr.Role)
                .WithMany(dr => dr.Departments)
                .HasForeignKey(dr => dr.RoleId);

            modelBuilder.Entity<DepartmentEmployee>()
                .HasKey(de => new { de.DepartmentId, de.EmployeeId });
            modelBuilder.Entity<DepartmentEmployee>()
                .HasOne(dr => dr.Department)
                .WithMany(de => de.Employees)
                .HasForeignKey(de => de.DepartmentId);
            modelBuilder.Entity<DepartmentEmployee>()
                .HasOne(de => de.Employee)
                .WithMany(de => de.Departments)
                .HasForeignKey(de => de.EmployeeId);

            modelBuilder.Entity<EmployeeManager>()
                .HasKey(em => new { em.EmployeeId, em.ManagerId });
            modelBuilder.Entity<EmployeeManager>()
                .HasOne(em => em.Employee)
                .WithMany(em => em.Managers)
                .HasForeignKey(em => em.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<EmployeeManager>()
                .HasOne(em => em.Manager)
                .WithMany(em => em.Employees)
                .HasForeignKey(em => em.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<EmployeeRole>()
                .HasKey(er => new { er.EmployeeId, er.RoleId });
            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Employee)
                .WithMany(er => er.Roles)
                .HasForeignKey(er => er.EmployeeId);
            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Role)
                .WithMany(er => er.Employees)
                .HasForeignKey(er => er.RoleId);

            modelBuilder.Entity<HospitalAddress>()
                .HasKey(ha => new { ha.HospitalId, ha.AddressId });
            modelBuilder.Entity<HospitalAddress>()
                .HasOne(ha => ha.Hospital)
                .WithMany(ha => ha.Addresses)
                .HasForeignKey(ha => ha.HospitalId);
            modelBuilder.Entity<HospitalAddress>()
                .HasOne(ha => ha.Address)
                .WithMany(ha => ha.Hospitals)
                .HasForeignKey(ha => ha.AddressId);

            modelBuilder.Entity<PersonAddress>()
                .HasKey(pa => new { pa.PersonId, pa.AddressId });
            modelBuilder.Entity<PersonAddress>()
                .HasOne(pa => pa.Person)
                .WithMany(pa => pa.Addresses)
                .HasForeignKey(pa => pa.PersonId);
            modelBuilder.Entity<PersonAddress>()
                .HasOne(pa => pa.Address)
                .WithMany(pa => pa.People)
                .HasForeignKey(pa => pa.AddressId);

            modelBuilder.Entity<AdmitAilment>()
                .HasKey(aa => new { aa.PatientAdmitId, aa.AilmentId });
            modelBuilder.Entity<AdmitAilment>()
                .HasOne(aa => aa.PatientAdmit)
                .WithMany(aa => aa.Ailments)
                .HasForeignKey(aa => aa.PatientAdmitId);
            modelBuilder.Entity<AdmitAilment>()
                .HasOne(aa => aa.Ailment)
                .WithMany(aa => aa.PatientAdmits)
                .HasForeignKey(aa => aa.AilmentId);
                
        }
    }
}
