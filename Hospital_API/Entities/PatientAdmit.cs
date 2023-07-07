namespace Hospital_API.Entities
{
    public class PatientAdmit
    {
        public int Id { get; set; }
        public DateTime AdmitDate { get; set; }
        public string? AdmitComment { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string? DischargeComment { get; set; }
        public string? Prescription { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        public int PatientId { get; set; }
        public Patient? Patient { get; set; }
        public int WardId { get; set; }
        public Ward? Ward { get; set; }

        public virtual ICollection<AdmitAilment>? Ailments { get; set; }
        public virtual ICollection<PatientTranfer>? PatientTranfers { get; set; }
    }
}
