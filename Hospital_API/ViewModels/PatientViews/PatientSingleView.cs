using Hospital_API.Entities;
using Hospital_API.ModelViews.PersonViews;
using Hospital_API.ViewModels;
using Newtonsoft.Json;

namespace Hospital_API.ModelViews.PatientViews
{
    public class PatientSingleView : PersonDetailView
    {
        [JsonProperty("patientAdmits")]
        public ICollection<PatientAdmitView>? PatientAdmits { get; set; }

        [JsonProperty("PatientTransfers")]
        public ICollection<PatientTransferView>? PatientTransfers { get; set; }
    }
}
