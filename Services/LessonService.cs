using DistantEdu.Data;
using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Types;
using DistantEdu.ViewModels;
using Humanizer;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;

namespace DistantEdu.Services
{
    public class LessonService
    {
        private readonly ApplicationDbContext _context;
        private readonly QuizService _quizService;
        private readonly ILogger<LessonService> _logger;
        public LessonService(ApplicationDbContext context, QuizService quizService, ILogger<LessonService> logger)
        {
            _context = context;
            _quizService = quizService;
            _logger = logger;
        }

        private async Task<LessonScore?> GetLessonScoreAsync(Lesson lesson, string userName)
        {            
            var profile = await _context.StudentProfiles.FirstAsync(sp => sp.Name == userName);
            var lessonScore = profile.SubjectSubscriptions
                .Where(ss => ss.SubjectId == lesson.SubjectId)
                .SelectMany(ss => ss.LessonScores)
                .FirstOrDefault(lessonScore => lessonScore.LessonId == lesson.Id) ??
                new LessonScore
                {
                    LessonId = lesson.Id,
                    IsPassed = false,
                    QuizScoresList = new List<QuizScore>(),
                    SubjectSubscriptionId = profile.SubjectSubscriptions.First(ss => ss.SubjectId == lesson.SubjectId).Id
                };

            await _context.SaveChangesAsync();
            return lessonScore;
        }
        public async Task<LessonViewModel?> GetLessonPerStudentAsync(int lessonId, string userName)
        {
            if (await _context.Lessons.FindAsync(lessonId) is not { } lesson || 
                await GetLessonScoreAsync(lesson, userName) is not { } lessonScore) return null;

            return MergeInLessonViewModel(lesson, lessonScore);
        }

        private LessonViewModel MergeInLessonViewModel(Lesson lesson, LessonScore lessonScore)
            => new LessonViewModel
            {
                LessonId = lesson.Id,
                LessonScoreId = lessonScore.Id,
                Name = lesson.Name,
                Description = lesson.Description,
                Order = lesson.Order,
                Content = lesson.Content,
                Condition = lesson.Condition,
                SubjectId = lesson.SubjectId,
                SubscriptionId = lessonScore.SubjectSubscriptionId,
                IsPassed = lessonScore.IsPassed,
                Quizzes = lesson.Tests.Where(quiz => quiz.Questions.Count >= quiz.Count)
                    .Select(q => _quizService.GetShallowQuizInfoAsync(q.Id, lessonScore.Id))
                    .Select(t => t.Result)
                    .OfType<QuizViewModel>()
                    .ToList()
            };

        public Lesson? GetLessonPerEdit(int lessonId)
        => _context.Lessons
            .Include(lesson => lesson.Tests)
            .FirstOrDefault(lesson => lesson.Id == lessonId);

        #region IS_PASSED

        private bool CheckPassed(IEnumerable<Quiz> quizzes, LessonScore lessonScore)
        {
            bool Passed = true;
            foreach (var quiz in quizzes)
            {
                Passed &= lessonScore.QuizScoresList.Where(qs => qs.QuizId == quiz.Id && qs.Score > 0).Any();
                if (!Passed) break;
            }

            return Passed;
        }

        private bool IsPassed(PassCondition condition, Lesson lesson, LessonScore lessonScore)
        {
            switch (condition)
            {
                case PassCondition.ReadOnly: 
                    return true;
                case PassCondition.SingleTest:
                    return lessonScore.QuizScoresList.Where(qs => qs.Score > 0).Any();
                case PassCondition.KeyTests:
                    var keyTests = from quiz in lesson.Tests 
                                   where quiz.QType == QuizType.Key || quiz.QType == QuizType.KeyHardcore
                                   select quiz;
                    return CheckPassed(keyTests, lessonScore);
                case PassCondition.AllTests:
                    return CheckPassed(lesson.Tests, lessonScore);
                default: return false;
            }
        }

        /// <summary>
        /// Checks if pass requirements met and changes IsPassed in LessonScore on true if yes.
        /// </summary>
        /// <param name="lessonScoreId">lesson score id for checking</param>
        /// <returns>True if requirements met</returns>
        /// <exception cref="NullReferenceException">If Lesson or LessonScore were not found</exception>
        public async Task<bool> DecideIfLessonPassedAsync(int lessonScoreId)
        {
            if (await _context.LessonScores.FindAsync(lessonScoreId) is not { } lessonScore)
                throw new NullReferenceException($"Lesson score with ID:{lessonScoreId} could not be found");
            if (await _context.Lessons.FindAsync(lessonScore.LessonId) is not { } lesson)
                throw new NullReferenceException($"Lesson with ID:{lessonScore.LessonId} not found");
            if (!IsPassed(lesson.Condition, lesson, lessonScore)) 
                return false;

            lessonScore.IsPassed = true;
            _context.Entry(lessonScore).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion
    }
}
