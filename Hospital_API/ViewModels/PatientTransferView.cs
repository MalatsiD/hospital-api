namespace Hospital_API.ViewModels
{
    public class PatientTransferView
    {
        public int Id { get; set; }
        public DateTime TransferDate { get; set; }
        public string? Reason { get; set; }
        public bool Active { get; set; }

        public int PatientAdmitId { get; set; }

        public int? SourceHospitalId { get; set; } //Source Hospital
        public string? SourceHospitalName { get; set; } //Source Hospital

        public int? SourceWardId { get; set; } //Source Ward
        public string? SourceWardName { get; set; } //Source Ward

        public int? DestinationHospitalId { get; set; } //Destination Hospital
        public string? DestinationHospitalName { get; set; } //Destination Hospital

        public int? DestinationWardId { get; set; } //Destination Ward
        public string? DestinationWardName { get; set; } //Destination Ward
    }
}
