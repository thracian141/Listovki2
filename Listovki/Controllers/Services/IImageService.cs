namespace Listovki.Controllers.Services {
    public interface IImageService {
        Task<string> SaveQuestionImgAndGetURL(IFormFile imageFile);
        Task<FileStream> GetImageUrlByQuestionId(int questionId);
    }
}
