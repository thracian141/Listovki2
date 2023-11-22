using System.ComponentModel.DataAnnotations;

namespace Listovki.Models {
    public class ExamQuestion {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Question { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public int Points { get; set; }
        [Required]
        public bool IsMultipleChoice { get; set; }
        public string? MediaURL { get; set; }
    }
}
