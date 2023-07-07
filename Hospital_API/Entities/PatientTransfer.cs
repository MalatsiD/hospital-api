namespace Hospital_API.Entities
{
    public class PatientTranfer
    {
        public int Id { get; set; }
        public DateTime TransferDate { get; set; }
        public string? Reason { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        public int PatientAdmitId { get; set; }
        public PatientAdmit? PatientAdmit { get; set; }

        public int? HospitalId { get; set; } //Destination Hospital
        public Hospital? Hospital { get; set; }

        public int? WardId { get; set; }
        public Ward? Ward { get; set; }
    }
}
