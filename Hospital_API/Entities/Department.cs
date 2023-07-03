namespace Hospital_API.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; }

        public ICollection<DepartmentRole> Roles { get; set; }
        public ICollection<DepartmentEmployee> Employees { get; set; }
        public ICollection<Ward> Wards { get; set; }
    }
}
