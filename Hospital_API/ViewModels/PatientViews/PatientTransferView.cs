using Newtonsoft.Json;

namespace Hospital_API.ViewModels.PatientViews
{
    public class PatientTransferView
    {
        [JsonProperty("patientTransferId")]
        public int PatientTransferId { get; set; }

        [JsonProperty("transferDate")]
        public DateTime TransferDate { get; set; }

        [JsonProperty("reason")]
        public string? Reason { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("patientAdmitId")]
        public int PatientAdmitId { get; set; }

        [JsonProperty("sourceHospitalId")]
        public int? SourceHospitalId { get; set; } //Source Hospital

        [JsonProperty("sourceHospitalName")]
        public string? SourceHospitalName { get; set; } //Source Hospital

        [JsonProperty("sourceWardId")]
        public int? SourceWardId { get; set; } //Source Ward

        [JsonProperty("sourceWardName")]
        public string? SourceWardName { get; set; } //Source Ward

        [JsonProperty("destinationHospitalId")]
        public int? DestinationHospitalId { get; set; } //Destination Hospital

        [JsonProperty("destinationHospitalName")]
        public string? DestinationHospitalName { get; set; } //Destination Hospital

        [JsonProperty("destinationWardId")]
        public int? DestinationWardId { get; set; } //Destination Ward

        [JsonProperty("destinationWardName")]
        public string? DestinationWardName { get; set; } //Destination Ward
    }
}
