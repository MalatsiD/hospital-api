namespace Hospital_API.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        public ICollection<DepartmentRole> Departments { get; set; }
        public ICollection<EmployeeRole> Employees { get; set; }
    }
}
