using Hospital_API.ViewModels.PersonViews;
using Newtonsoft.Json;

namespace Hospital_API.ViewModels.PatientViews
{
    public class PatientTableView : PersonTableView
    {
        [JsonProperty("patientId")]
        public int PatientId { get; set; }

        [JsonProperty("patientNumber")]
        public string? PatientNumber { get; set; }
    }
}
