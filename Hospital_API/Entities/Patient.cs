namespace Hospital_API.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string? PatientNumber { get; set; } //This should be auto generated
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        public int PersonId { get; set; }
        public virtual Person? Person { get; set; }

        public virtual ICollection<PatientAdmit>? PatientAdmits { get; set; }
    }
}
