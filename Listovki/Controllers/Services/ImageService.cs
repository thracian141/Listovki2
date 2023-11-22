using Listovki.Data;

namespace Listovki.Controllers.Services {
    public class ImageService : IImageService {
        private IWebHostEnvironment _env;
        private ApplicationDbContext _db;

        public ImageService(IWebHostEnvironment env, ApplicationDbContext db) {
            _env = env;
            _db = db;
        }
        public async Task<string> SaveQuestionImgAndGetURL(IFormFile imageFile) {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            string filePath = Path.Combine(_env.WebRootPath, "QuestionImages");
            string fullFilePath = Path.Combine(filePath, uniqueFileName);

            using (var stream = new FileStream(fullFilePath, FileMode.Create)) {
                await imageFile.CopyToAsync(stream);
            }

            return uniqueFileName;
        }

        public async Task<FileStream> GetImageUrlByQuestionId(int questionId)
        {
            var question = await _db.ExamQuestions.FindAsync(questionId);
            string filePath = Path.Combine(_env.WebRootPath, "QuestionImages");
            string imgUrl = Path.Combine(filePath, question.MediaURL);
            var image = System.IO.File.OpenRead(imgUrl);

            return image;
        }
    }
}
