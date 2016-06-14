using System.ComponentModel.DataAnnotations;

namespace MeeToo.Domain
{
    public class Option
    {
        [Required]
        public string Id { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }
}
