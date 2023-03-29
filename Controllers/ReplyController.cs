using DistantEdu.Data;
using DistantEdu.Services;
using DistantEdu.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.EventSource;
using NuGet.Protocol.Plugins;

namespace DistantEdu.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize]
    public class ReplyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly QuizService _quizService;
        private readonly LessonService _lessonService;
        public ReplyController(ApplicationDbContext context, QuizService quizService, LessonService lessonService)
        {
            _context = context;
            _quizService = quizService;
            _lessonService = lessonService;
        }

        [HttpPost]
        public async Task<ActionResult> AnswerOnQuery(int quizScoreId, [FromBody] List<AnswerMessage> answers)
        {
            if (await _context.QuizScores.FindAsync(quizScoreId)is not { } quizScore) 
                return BadRequest(new { message = $"quiz with {quizScoreId} is not found" }); 
            if (await _context.Quizzes.FindAsync(quizScore.QuizId) is not { } quiz)
                return BadRequest(new { message = $"original quiz not found [{quizScore.QuizId}]" });

            if (quizScore.StartTime + quiz.Duration > DateTimeOffset.UtcNow)
            {
                _ = await _quizService.FinishQuizAsync(quizScoreId);
                await _lessonService.DecideIfLessonPassed(quizScore.LessonScoreId);
                return BadRequest(new { message = "replies rejected. Cause: time expired" });
            }

            await _quizService.Reply(quizScoreId, answers);
            return Ok();
        }
    }
}
