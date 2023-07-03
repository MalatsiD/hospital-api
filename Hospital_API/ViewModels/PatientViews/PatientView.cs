using Hospital_API.ModelViews.PersonViews;
using Hospital_API.ViewModels.PatientViews;
using Newtonsoft.Json;

namespace Hospital_API.ModelViews.PatientViews
{
    public class PatientView : PersonView
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("patientNumber")]
        public string? PatientNumber { get; set; }

        [JsonProperty("fullName")]
        public string? FullName { get; set; }

        public virtual ICollection<PatientAddressView>? Addresses { get; set; }
    }
}
