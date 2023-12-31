﻿namespace Hospital_API.Entities
{
    public class DepartmentEmployee
    {
        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }
    }
}
