using Newtonsoft.Json;

namespace Hospital_API.ViewModels.PersonViews
{
    public class PersonSingleView : PersonTableView
    {
        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("middleName")]
        public string? MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }

        public virtual ICollection<AddressView>? Addressess { get; set; }
    }
}
