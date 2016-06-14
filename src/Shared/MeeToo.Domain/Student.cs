using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using MeeToo.Domain.Attributes;
using MeeToo.Domain.Contracts;

namespace MeeToo.Domain
{
    [DbCollection("students")]
    [JsonObject(MemberSerialization.OptIn)]
    public class Student : User, IDocument
    {
        [JsonProperty("birthDate")]
        public DateTimeOffset BirthDate { get; set; }

        [JsonProperty("bookNumber")]
        public string BookNumber { get; set; }

        [Required]
        [JsonProperty("group")]
        public string Group { get; set; }

        [Range(0, 100)]
        [JsonProperty("grade")]
        public int Grade { get; set; }

        [JsonProperty("testIds")]
        public List<Guid> TestIds { get; set; }
    }
}
