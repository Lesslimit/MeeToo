using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MeeToo.Domain
{
    public class Question
    {
        [Required]
        public string Id { get; set; }

        public string Title { get; set; }

        public IList<Option> Options { get; set; }

        public Option CorrectAnswer => Options.SingleOrDefault(o => o.IsCorrect);
    }
}
