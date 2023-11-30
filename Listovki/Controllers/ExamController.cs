using Listovki.Controllers.Services;
using Listovki.Data;
using Listovki.Models;
using Listovki.Models.DataModels;
using Microsoft.AspNetCore.Identity;
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
        private UserManager<AppUser> _userManager;
        public ExamController(ApplicationDbContext db, IImageService img,
            ILogger<ExamController> logger, UserManager<AppUser> userManager)
        {
            _db = db;
            _img = img;
            _logger = logger;
            _userManager = userManager;
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
        [HttpPost("gradeExam")]
        public async Task<IActionResult> GradeExam([FromBody] ExamInputModel listovkaModel) {
            var user = await _userManager.GetUserAsync(User);
            var exam = listovkaModel.Questions;

            ListovkaResult result = new ListovkaResult {
                Id = 0,
                PercentageRight = 0,
                QuestionsNumber = 0,
                GuessedQuestionsNumber = 0,
                Category = "F",
                UserEmail = user.Email,
                User = user
            };
            double pointsTotal = 0;
            double pointsGuessed = 0;
            int questionsGuessedCount = 0;
            string category = listovkaModel.Category;

            foreach (var question in exam) {
                var itsQuestion = await _db.ExamQuestions.FindAsync(question.questionId);
                var itsAnswers = await _db.Answers.Where(a => a.ExamQuestionId == question.questionId).ToListAsync();

                bool guessedRight = true;
                if (itsQuestion.IsMultipleChoice) {
                    foreach (var answer in itsAnswers) {
                        if (question.answers[answer.Id] != answer.IsCorrect) {
                            guessedRight = false;
                        }
                    }
                } else {
                    foreach (var answer in itsAnswers) {
                        var correctAnswer = itsAnswers.Where(a => a.IsCorrect == true).FirstOrDefault();
                        if (question.answers[answer.Id] != answer.IsCorrect) {
                            guessedRight = false;
                        }
                    }
                }
                if (guessedRight) {
                    pointsGuessed += itsQuestion.Points;
                    questionsGuessedCount++;
                }
                pointsTotal += itsQuestion.Points;
            }

            double grade = (pointsGuessed / pointsTotal) * 100;

            result.PercentageRight = grade;
            result.QuestionsNumber = exam.Count;
            result.GuessedQuestionsNumber = questionsGuessedCount;
            result.Category = category;

            await _db.ListovkaResults.AddAsync(result);
            await _db.SaveChangesAsync();

            return Ok(result.Id);
        }

        [HttpGet("results")]
        public async Task<IActionResult> Results(int listovkaId)
        {
            var listovka = await _db.ListovkaResults.FindAsync(listovkaId);

            if (listovka == null)
            {
                return NotFound("Listovka not found!");
            }

            return new JsonResult(new { listovka });
        }
        [HttpGet("stats")]
        public async Task<IActionResult> Start()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Dictionary<string, int> stats = new Dictionary<string, int>();
                var listovki = await _db.ListovkaResults.Where(l => l.UserEmail == user.Email).ToListAsync();

                if (listovki != null)
                {
                    int listovkiCount = listovki.Count;
                    int verniListovki = listovki.Count(l => l.PercentageRight >= 89);

                    int aCatListovki = listovki.Count(l => l.Category == "A");
                    int bCatListovki = listovki.Count(l => l.Category == "B");
                    int cCatListovki = listovki.Count(l => l.Category == "C");
                    int dCatListovki = listovki.Count(l => l.Category == "D");

                    int aCatVerniListovki = listovki.Count(l => l.Category == "A" && l.PercentageRight >= 89);
                    int bCatVerniListovki = listovki.Count(l => l.Category == "B" && l.PercentageRight >= 89);
                    int cCatVerniListovki = listovki.Count(l => l.Category == "C" && l.PercentageRight >= 89);
                    int dCatVerniListovki = listovki.Count(l => l.Category == "D" && l.PercentageRight >= 89);

                    int lastExamId = listovki.Last().Id;

                    stats.Add("listovkiCount", listovkiCount);
                    stats.Add("verniListovki", verniListovki);

                    stats.Add("aCatListovki", aCatListovki);
                    stats.Add("bCatListovki", bCatListovki);
                    stats.Add("cCatListovki", cCatListovki);
                    stats.Add("dCatListovki", dCatListovki);

                    stats.Add("aCatVerniListovki", aCatVerniListovki);
                    stats.Add("bCatVerniListovki", bCatVerniListovki);
                    stats.Add("cCatVerniListovki", cCatVerniListovki);
                    stats.Add("dCatVerniListovki", dCatVerniListovki);

                    stats.Add("lastExamId", lastExamId);
                }

                return new JsonResult(new { stats });
            }
            return NoContent();
        }
    }
}
