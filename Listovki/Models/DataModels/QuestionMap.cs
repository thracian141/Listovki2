namespace Listovki.Models.DataModels {
    public class QuestionMap {
        public int questionId { get; set; }
        public Dictionary<int, bool> answers { get; set; }

    }
}
