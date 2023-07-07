using Hospital_API.ViewModels.PersonViews;
using Newtonsoft.Json;

namespace Hospital_API.ViewModels.EmployeeViews
{
    public class EmployeeTableView : PersonTableView
    {
        [JsonProperty("employeeId")]
        public int EmployeeId { get; set; }

        [JsonProperty("departmentId")]
        public int DepartmentId { get; set; }

        [JsonProperty("departmentName")]
        public string? DepartmentName { get; set; }

        [JsonProperty("roleId")]
        public int RoleId { get; set; }

        [JsonProperty("roleName")]
        public string? RoleName { get; set; }

        [JsonProperty("managerId")]
        public int ManagerId { get; set; }

        [JsonProperty("managerName")]
        public string? ManagerName { get; set; }
    }
}
