using System;
using Newtonsoft.Json;

namespace MeeToo.Api.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserRegistrationModel
    {
        [JsonProperty]
        public string Email { get; set; }

        [JsonProperty]
        public string Password { get; set; }

        [JsonProperty]
        public string PhoneNumber { get; set; }

        [JsonProperty]
        public string BookNumber { get; set; }

        [JsonProperty]
        public DateTimeOffset BirthDate { get; set; }
    }
}
