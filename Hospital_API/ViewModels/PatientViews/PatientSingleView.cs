using Hospital_API.Entities;
using Hospital_API.ModelViews.PersonViews;
using Hospital_API.ViewModels.PatientViews;
using Hospital_API.ViewModels.PersonViews;
using Newtonsoft.Json;

namespace Hospital_API.ModelViews.PatientViews
{
    public class PatientSingleView : PersonSingleView
    {
        [JsonProperty("patientId")]
        public int PatientId  { get; set; }

        [JsonProperty("patientNumber")]
        public string? PatientNumber { get; set; }

        [JsonProperty("patientAdmits")]
        public ICollection<PatientAdmitView>? PatientAdmits { get; set; }

        //[JsonProperty("PatientTransfers")]
        //public ICollection<PatientTransferView>? PatientTransfers { get; set; }
    }
}
