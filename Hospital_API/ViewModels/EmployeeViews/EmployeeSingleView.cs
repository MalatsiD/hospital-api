using Hospital_API.ViewModels.PersonViews;
using Newtonsoft.Json;

namespace Hospital_API.ViewModels.EmployeeViews
{
    public class EmployeeSingleView : PersonSingleView
    {
        [JsonProperty("employeeId")]
        public int EmployeeId { get; set; }

        [JsonProperty("hireDate")]
        public DateTime HireDate { get; set; }

        [JsonProperty("terminationDate")]
        public DateTime TerminationDate { get; set; }

        [JsonProperty("departments")]
        public virtual ICollection<DepartmentView>? Departments { get; set; }

        [JsonProperty("managers")]
        public virtual ICollection<ManagerView>? Managers { get; set; }

        [JsonProperty("roles")]
        public virtual ICollection<RoleView>? Roles { get; set; }
    }
}
