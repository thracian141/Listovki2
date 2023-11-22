using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Listovki.Models {
    public class Answer {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public bool IsCorrect { get; set; }
        [Required]
        public int ExamQuestionId { get; set; }
        [ForeignKey("ExamQuestionId")]
        public virtual ExamQuestion ExamQuestion { get; set; }
    }
}
