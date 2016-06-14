using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MeeToo.Domain.Attributes;
using MeeToo.Domain.Contracts;

namespace MeeToo.Domain
{
    [DbCollection("tests")]
    public class Test : IDocument
    {
        public Test()
        {
            Questions = new List<Question>();
        }

        [Required]
        public string Id { get; set; }

        [Range(0, 100)]
        public int Result { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public bool IsComplete { get; set; }

        public IList<Question> Questions { get; set; }
    }
}
