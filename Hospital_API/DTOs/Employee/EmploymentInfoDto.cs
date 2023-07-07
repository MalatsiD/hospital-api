using Newtonsoft.Json;

namespace Hospital_API.DTOs.Employee
{
    public class EmploymentInfoDto
    {
        [JsonProperty("managerId")]
        public int ManagerId { get; set; }

        [JsonProperty("departmentId")]
        public int DepartmentId { get; set; }

        [JsonProperty("roleId")]
        public int RoleId { get; set; }

        [JsonProperty("employmentDate")]
        public DateTime EmploymentDate { get; set; }
    }
}
