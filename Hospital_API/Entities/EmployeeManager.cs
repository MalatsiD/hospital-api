namespace Hospital_API.Entities
{
    public class EmployeeManager
    {
        public int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
        public int ManagerId { get; set; }
        public virtual Employee? Manager { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }
    }
}
