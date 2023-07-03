namespace Hospital_API.Entities
{
    public class Ward
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        public virtual ICollection<PatientAdmit>? PatientAdmits { get; set; }
        public virtual ICollection<PatientTranfer>? PatientTranfers { get; set; }
    }
}
