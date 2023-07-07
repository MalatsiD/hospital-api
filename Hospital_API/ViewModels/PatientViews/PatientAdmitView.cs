using Hospital_API.Entities;
using Newtonsoft.Json;

namespace Hospital_API.ViewModels.PatientViews
{
    public class PatientAdmitView
    {
        [JsonProperty("patientAdmitId")]
        public int PatientAdmitId { get; set; }

        [JsonProperty("admitDate")]
        public DateTime AdmitDate { get; set; }

        [JsonProperty("admitComment")]
        public string? AdmitComment { get; set; }

        [JsonProperty("dischargeDate")]
        public DateTime DischargeDate { get; set; }

        [JsonProperty("dischargeComment")]
        public string? DischargeComment { get; set; }

        [JsonProperty("prescription")]
        public string? Prescription { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("patientId")]
        public int PatientId { get; set; }

        [JsonProperty("fullName")]
        public string? FullName { get; set; }

        [JsonProperty("patientNumber")]
        public string? PatientNumber { get; set; }

        [JsonProperty("wardId")]
        public int WardId { get; set; }

        [JsonProperty("wardName")]
        public string? WardName { get; set; }

        public virtual ICollection<AdmitAilmentView>? Ailments { get; set; }
        public virtual ICollection<PatientTransferView>? PatientTransfers { get; set; }
    }
}
