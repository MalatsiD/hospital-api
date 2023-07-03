using Hospital_API.ViewModels;
using Newtonsoft.Json;

namespace Hospital_API.ModelViews.PersonViews
{
    public class PersonDetailView : PersonView
    {
        [JsonProperty("addresses")]
        public ICollection<AddressView>? Addresses { get; set; }
    }
}
