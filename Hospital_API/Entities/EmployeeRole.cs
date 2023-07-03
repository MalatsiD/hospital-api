namespace Hospital_API.Entities
{
    public class EmployeeRole
    {
        public int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }
    }
}
