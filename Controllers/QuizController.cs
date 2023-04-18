using DistantEdu.Data;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly QuizService _quizService;
        public QuizController(ApplicationDbContext context, QuizService quizService)
        {
            _context = context;
            _quizService = quizService;
        }

        [HttpPost]
        public async Task<ActionResult> PostNewQuiz([FromBody] Quiz quiz)
        {
            List<Query> queriesWithErrors = new();
            foreach (Query query in quiz.Questions)
            {
                // if there is no replies
                if (query.Replies.Count == 0)
                {
                    queriesWithErrors.Add(query);
                    continue;
                }

                // if Count > reolies or count == 0 (to avoid zero division, + it useless): then count = count of replies;
                if (query.Count > query.Replies.Count || query.Count == 0)
                    query.Count = query.Replies.Count;
            }

            quiz.Questions = quiz.Questions.Except(queriesWithErrors).ToList();
            bool isReadyForUse = quiz.Questions.Count >= quiz.Count;
            if (await _context.Lessons.FirstOrDefaultAsync(les => les.Id == quiz.LessonId) is not {} lesson)
                return BadRequest($"Lesson with Id [{quiz.LessonId}] not found");
            lesson.Tests.Add(quiz);
            await _context.SaveChangesAsync();
            return Ok(new {
                isReady = isReadyForUse,
                removed_questions = queriesWithErrors.Select(q => q.Content)
            });
        }

        // args = quizId, lessonScoreId?
        [HttpGet]
        public async Task<ActionResult> GetQuizInfo(string args)
        {
            int[] intArray = args.Split(':').Select(int.Parse).ToArray();
            if (intArray.Length > 1)
                return Ok(await _quizService.GetShallowQuizInfoAsync(intArray[0], intArray[1]));
            return Ok(await _context.Quizzes
                                .Include(q => q.Questions)
                                .ThenInclude(qt => qt.Replies)
                                .FirstOrDefaultAsync(q => q.Id == intArray[0])
                            );
        }

        [HttpPut]
        public async Task<ActionResult> UpdateQuiz([FromBody] Quiz quiz){
            _context.Quizzes.Update(quiz);
            await _context.SaveChangesAsync();
            return Accepted();
        }
    }
}
