namespace Hospital_API.Entities
{
    public class Hospital
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? RegistrationNumber { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Department>? Departments { get; set; }
        public virtual ICollection<HospitalAddress>? Addresses { get; set; }
        public virtual ICollection<PatientTranfer>? PatientTranfers { get; set; }
    }
}
