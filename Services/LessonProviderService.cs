using DistantEdu.Data;
using DistantEdu.Models;
using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DistantEdu.Services
{
    public class LessonProviderService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LessonProviderService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // selecting only those, where present more questions than specified in 
        private async Task<List<Quiz>> GetAvailableQuizesAsync(int lessonId)
            => await _context.Quizs.Include(q => q.Questions)
            .Where(q => q.LessonId == lessonId && q.Questions.Count >= q.Count)
            .ToListAsync();

        private List<QuizScore> GetScoresForQuizes(List<Quiz> quizes)
        {
            List<QuizScore> scores = new();
            foreach (var quiz in quizes)
            {
                var qs = _context.QuizScores.Where(qs => qs.QuizId == quiz.Id).ToList();
                if (qs is not null && qs.Count > 0)
                    scores.AddRange(qs);
            }

            return scores;
        }

        /// <summary>
        /// Retrieves lesson for user. Depending on users role returns either lesson itself with all quizes
        /// (including unfinished or lesson + it's progress and ready-for-use quizes with scores belonged to them
        /// </summary>
        /// <param name="lessonId"></param>
        /// <param name="userClaims"></param>
        /// <returns></returns>
        public async Task<object?> GetLessonFor(int lessonId, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return null;

            if (await _userManager.IsInRoleAsync(user, Roles.Student))
                return GetLessonPerStudent(lessonId, userName);
            return GetLessonPerEdit(lessonId);
        }

        private object GetLessonPerStudent(int lessonId, string userName)
            => from lesson in _context.Lessons
               where lesson.Id == lessonId
               join lessonScore in _context.LessonScores on lesson.Id equals lessonScore.LessonId
               select new
               {
                   lessonId,
                   name = lesson.Name,
                   description = lesson.Description,
                   order = lesson.Order,
                   content = lesson.Content,
                   passCondition = lesson.Condition,
                   subjectId = lesson.SubjectId,
                   subscriptionId = lessonScore.SubjectSubscriptionId,
                   isPassed = lessonScore.IsPassed,
                   scoreId = lessonScore.Id,
                   // Here selecting only full quizes
                   quizes = from quiz in lesson.Tests
                            where quiz.Count >= quiz.Questions.Count
                            select quiz,
                   quizScores = lessonScore.QuizScoresList.ToList(),
               };

        private object? GetLessonPerEdit(int lessonId)
        => _context.Lessons
            .Include(lesson => lesson.Tests)
            .FirstOrDefault(lesson => lesson.Id == lessonId);
    }
}
