using Newtonsoft.Json;

namespace Hospital_API.ViewModels
{
    public class SearchVM
    {
        [JsonProperty("paramName")]
        public string? ParamName { get; set; }

        [JsonProperty("paramValue")]
        public string? ParamValue { get; set; }
    }
}
