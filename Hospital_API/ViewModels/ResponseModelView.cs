using Newtonsoft.Json;

namespace Hospital_API.ViewModels
{
    public class ResponseModelView
    {
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; } = 500;

        [JsonProperty("errorMessage")]
        public string? ErrorMessage { get; set; } = "An internal error has occured!";

        [JsonProperty("response")]
        public object? Response { get; set; }
    }
}
