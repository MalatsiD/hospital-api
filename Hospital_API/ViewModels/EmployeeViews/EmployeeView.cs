using Hospital_API.ModelViews.PersonViews;
using Hospital_API.ViewModels.EmployeeViews;
using Newtonsoft.Json;

namespace Hospital_API.ModelViews.EmployeeViews
{
    public class EmployeeView : PersonView
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string? FullName { get; set; }

        [JsonProperty("hireDate")]
        public DateTime HireDate { get; set; }

        [JsonProperty("terminationDate")]
        public DateTime TerminationDate { get; set; }

        public virtual ICollection<ManagerView>? Managers { get; set; }
        public virtual ICollection<EmployeeAddressView>? Addresses { get; set; }
    }
}
