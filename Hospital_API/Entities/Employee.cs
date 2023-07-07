namespace Hospital_API.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        public int PersonId { get; set; }
        public virtual Person? Person { get; set; }

        public virtual ICollection<DepartmentEmployee>? Departments { get; set; }
        public virtual ICollection<EmployeeManager>? Employees { get; set; }
        public virtual ICollection<EmployeeManager>? Managers { get; set; }
        public virtual ICollection<EmployeeRole>? Roles { get; set; }
    }
}
