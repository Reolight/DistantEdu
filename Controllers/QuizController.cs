using AutoMapper.Configuration.Annotations;
using DistantEdu.Data;
using DistantEdu.Models;
using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DistantEdu.Controllers
{
    /// <summary>
    /// Controller for managing tests
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class QuizController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly QuizService _quizService;
        private readonly LessonService _lessonService;
        public QuizController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, QuizService quizService, LessonService lessonService)
        {
            _context = context;
            _userManager = userManager;
            _quizService = quizService;
            _lessonService = lessonService;
        }

        [HttpPost]
        public async Task<ActionResult> PostNewQuiz(int lessonId, [FromBody] Quiz quiz)
        {
            List<Query> queriesWithErrors = new();
            foreach (Query query in quiz.Questions)
            {
                if (query.Count > query.Replies.Count)
                    queriesWithErrors.Add(query);
            }

            quiz.Questions = quiz.Questions.Except(queriesWithErrors).ToList();
            bool isReadyForUse = quiz.Questions.Count >= quiz.Count;
            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();
            return Ok(new {
                isReady = isReadyForUse,
                removed_questions = queriesWithErrors.Select(q => q.Content),
                posted = quiz
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetQuizInfo(int lessonScoreId, int quizId)
        {
            var shallowQuizInfo = await _quizService.GetShallowQuizInfoAsync(lessonScoreId, quizId);
            return Ok(shallowQuizInfo);
        }

        [HttpPost]
        public async Task<ActionResult> StartQuiz(int lessonScoreId, int quizId)
        {
            var startetQuiz = await _quizService.StartNewQuizAsync(quizId, lessonScoreId);
            return startetQuiz is not null? Ok(startetQuiz) : BadRequest("quest can not be started");
        }

        [HttpPost]
        public async Task<ActionResult> FinishQuest(int quizScoreId)
        {
            if (await _context.QuizScores.FindAsync(quizScoreId) is not { } quizScore)
                return BadRequest(new { message = $"quiz with {quizScoreId} is not found" });
            _ = await _quizService.FinishQuizAsync(quizScoreId);
            await _lessonService.DecideIfLessonPassed(quizScore.LessonScoreId);
            return Ok(new { message = "Finished", redirect = $"quiz\\{quizScoreId}" });
        }
    }
}
