using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Listovki.Models.DataModels {
    public class AnswerInputModel {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int ExamQuestionId { get; set; }
    }
}
