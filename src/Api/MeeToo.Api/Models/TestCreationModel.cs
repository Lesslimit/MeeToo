using System;
using System.Collections.Generic;

namespace MeeToo.Api.Models
{
    public class TestCreationModel
    {
        public TimeSpan Duration { get; set; }
        public IEnumerable<QuestionCreationModel> Questions { get; set; }
    }
}
