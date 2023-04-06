using DistantEdu.Data;
using DistantEdu.Models;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Services;
using DistantEdu.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DistantEdu.Controllers
{
    /// <summary>
    /// Controller for managing tests
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AnswerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly QuizService _quizService;
        private readonly LessonService _lessonService;
        public AnswerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, QuizService quizService, LessonService lessonService)
        {
            _context = context;
            _userManager = userManager;
            _quizService = quizService;
            _lessonService = lessonService;
        }

        [HttpGet]
        public async Task<ActionResult> StartQuiz(int lessonScoreId, int quizId)
        {
            var startetQuiz = await _quizService.StartNewQuizAsync(quizId, lessonScoreId);
            return startetQuiz is not null? Ok(startetQuiz) : BadRequest("quest can not be started");
        }

        // Updates not in classic way. Calculates internal state and then updates
        [HttpPut]
        public async Task<ActionResult> FinishQuest(int quizScoreId)
        {
            if (await _context.QuizScores.FindAsync(quizScoreId) is not { } quizScore)
                return BadRequest(new { message = $"quiz with {quizScoreId} is not found" });
            _ = await _quizService.FinishQuizAsync(quizScoreId);
            await _lessonService.DecideIfLessonPassedAsync(quizScore.LessonScoreId);
            return Ok(new { message = "Finished", redirect = $"quiz\\{quizScoreId}" });
        }

        [HttpPost]
        public async Task<ActionResult> AnswerOnQuery(int quizScoreId, [FromBody] List<AnswerMessage> answers)
        {
            if (await _context.QuizScores.FindAsync(quizScoreId)is not { } quizScore) 
                return BadRequest(new { message = $"quiz with {quizScoreId} is not found" }); 
            if (await _context.Quizzes.FindAsync(quizScore.QuizId) is not { } quiz)
                return BadRequest(new { message = $"original quiz not found [{quizScore.QuizId}]" });

            if (quizScore.StartTime + TimeSpan.FromMinutes(quiz.Duration) > DateTimeOffset.UtcNow)
            {
                _ = await _quizService.FinishQuizAsync(quizScoreId);
                await _lessonService.DecideIfLessonPassedAsync(quizScore.LessonScoreId);
                return BadRequest(new { message = "replies rejected. Cause: time expired" });
            }

            await _quizService.Reply(quizScoreId, answers);
            return Ok();
        }
    }
}