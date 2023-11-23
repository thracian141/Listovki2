using Listovki.Controllers.Services;
using Listovki.Data;
using Listovki.Models;
using Listovki.Models.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Listovki.Controllers {
    [ApiController]
    [Route("exam")]
    public class ExamController : Controller {
        private ApplicationDbContext _db;
        private IImageService _img;
        private ILogger<ExamController> _logger;
        public ExamController(ApplicationDbContext db, IImageService img, ILogger<ExamController> logger)
        {
            _db = db;
            _img = img;
            _logger = logger;
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

                int id = examQuestion.Id;

                _logger.LogInformation("ID IS: " + id);
                return Ok(id);
            }

            return NotFound("Failed to upload image!");
        }
        [HttpPost("addAnswer")]
        public async Task<IActionResult> AddAnswers([FromBody] AnswerInputModel inputModel) {
            if (inputModel != null) {
                Answer answer = new Answer {
                    Id = 0,
                    Text = inputModel.Text,
                    IsCorrect = inputModel.IsCorrect,
                    ExamQuestionId = inputModel.ExamQuestionId,
                    ExamQuestion = await _db.ExamQuestions.FindAsync(inputModel.ExamQuestionId)
                };
                await _db.Answers.AddAsync(answer);
                await _db.SaveChangesAsync();

                return Ok();
            }
            return NotFound();
        }
        [HttpGet("listAnswersByQuestionId")]
        public async Task<IActionResult> ListAnswersByQuestionId(int questionId) {
            var answers = await _db.Answers.Where(a => a.ExamQuestionId == questionId).ToListAsync();

            return new JsonResult(new { answers });
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
        [HttpGet("getExamByCategory")]
        public async Task<IActionResult> GetExamByCategory(string category) {
            var questions = await _db.ExamQuestions.Where(q => q.Category == category).ToListAsync();
            //log info 
            _logger.LogInformation("got questions!");
            var random = new Random();
            questions = questions.OrderBy(q => random.Next()).ToList();
            _logger.LogInformation("randomized");
            questions = questions.Take(40).ToList();
            _logger.LogInformation("took 40");


            return new JsonResult(new {questions });
        }
    }
}
