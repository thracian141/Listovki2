namespace Listovki.Models.DataModels {
    public class QuestionInputModel {
        public int Id { get; set; }
        public string Question { get; set; }
        public int Points { get; set; }
        public string Category { get; set; }
        public bool IsMultipleChoice { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
