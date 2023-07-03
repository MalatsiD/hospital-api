using Newtonsoft.Json;

namespace Hospital_API.ViewModels.EmployeeViews
{
    public class EmployeeAddressView : AddressView
    {
        [JsonProperty("employeeId")]
        public int EmployeeId { get; set; }
    }
}
