using Newtonsoft.Json;

namespace Hospital_API.DTOs.Patient
{
    public class PatientDischargeDto
    {
        [JsonProperty("PatientAdmitId")]
        public int PatientAdmitId { get; set; }

        [JsonProperty("DischargeDate")]
        public DateTime? DischargeDate { get; set; }

        [JsonProperty("DischargeComment")]
        public string? DischargeComment { get; set; }

        [JsonProperty("Prescription")]
        public string? Prescription { get; set; }
    }
}
