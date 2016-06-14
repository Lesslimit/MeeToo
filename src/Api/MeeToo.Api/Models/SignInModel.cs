using Newtonsoft.Json;

namespace MeeToo.Api.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SignInModel
    {
        [JsonProperty]
        public string Email { get; set; }

        [JsonProperty]
        public string Password { get; set; }
    }
}
