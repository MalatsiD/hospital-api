using Hospital_API.ModelViews.PersonViews;
using Newtonsoft.Json;

namespace Hospital_API.ViewModels.EmployeeViews
{
    public class ManagerView
    {
        [JsonProperty("employeeId")]
        public int EmployeeId { get; set; }

        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("middleName")]
        public string? MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        [JsonProperty("fullName")]
        public string? FullName { get; set; }
    }
}
