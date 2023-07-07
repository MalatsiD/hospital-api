using Hospital_API.ViewModels;
using Hospital_API.ViewModels.PatientViews;
using Newtonsoft.Json;

namespace Hospital_API.ModelViews.PersonViews
{
    public class PersonView
    {
        [JsonProperty("personId")]
        public int PersonId { get; set; }

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

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("titleId")]
        public int TitleId { get; set; }

        [JsonProperty("titleName")]
        public string? TitleName { get; set; }

        [JsonProperty("genderId")]
        public int GenderId { get; set; }

        [JsonProperty("genderName")]
        public string? GenderName { get; set; }
    }
}
