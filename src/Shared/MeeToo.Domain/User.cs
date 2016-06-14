using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MeeToo.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class User
    {
        [Required]
        [JsonProperty("id")]
        public string Id { get; set; }

        public string Email => Id;

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [Required]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public  string MiddleName { get; set; }

        [Required]
        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
}
