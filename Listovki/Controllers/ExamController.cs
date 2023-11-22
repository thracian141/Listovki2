using Listovki.Controllers.Services;
using Listovki.Data;
using Listovki.Models;
using Listovki.Models.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Listovki.Controllers {
    [ApiController]
    [Route("exam")]
    public class ExamController : Controller {
        private ApplicationDbContext _db;
        private IImageService _img;
        public ExamController(ApplicationDbContext db, IImageService img)
        {
            _db = db;
            _img = img;
        }


        [HttpPost("addQuestion")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddQuestion([FromForm] QuestionInputModel inputModel) {
            if (inputModel != null) {
                var imgurl = await _img.SaveQuestionImgAndGetURL(inputModel.ImageFile);
                ExamQuestion examQuestion = new ExamQuestion {
                    Id = 0,
                    Question = inputModel.Question,
                    Category = inputModel.Category,
                    Points = inputModel.Points,
                    IsMultipleChoice = inputModel.IsMultipleChoice,
                    MediaURL = imgurl
                };

                await _db.ExamQuestions.AddAsync(examQuestion);
                await _db.SaveChangesAsync();

                return Ok();
            }

            return NotFound("Failed to upload image!");
        }
        [HttpGet("listQuestions")]
        public async Task<IActionResult> ListQuestions() {
            var questions = await _db.ExamQuestions.ToListAsync();

            return new JsonResult(new { questions });
        }
        [HttpGet("getQuestionImg")]
        public async Task<IActionResult> GetQuestionImg(int questionId)
        {
            var img = await _img.GetImageUrlByQuestionId(questionId);

            return File(img, "image/png");
        }
    }
}
