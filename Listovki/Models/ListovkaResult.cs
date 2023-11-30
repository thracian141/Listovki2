using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Listovki.Models {
    public class ListovkaResult {
        [Key]
        public int Id { get; set; }
        [Required]
        public double PercentageRight { get; set; }
        [Required]
        public int QuestionsNumber { get; set; }
        [Required]
        public int GuessedQuestionsNumber { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [ForeignKey("UserEmail")]
        public virtual AppUser User { get; set; }
    }
}
