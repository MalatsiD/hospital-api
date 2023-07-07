using Newtonsoft.Json;

namespace Hospital_API.ViewModels.PersonViews
{
    public class PersonTableView
    {
        [JsonProperty("personId")]
        public int PersonId { get; set; }

        [JsonProperty("fullName")]
        public string? FullName { get; set; }

        [JsonProperty("titleId")]
        public int TitleId { get; set; }

        [JsonProperty("titleName")]
        public string? TitleName { get; set; }

        [JsonProperty("genderId")]
        public int GenderId { get; set; }

        [JsonProperty("genderName")]
        public string? GenderName { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
