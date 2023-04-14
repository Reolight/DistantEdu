using DistantEdu.Data;
using DistantEdu.Models;
using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Types;
using DistantEdu.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;

namespace DistantEdu.Services
{
    public class LessonService
    {
        private readonly ApplicationDbContext _context;
        private readonly QuizService _quizService;
        private readonly ILogger<LessonService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public LessonService(ApplicationDbContext context, QuizService quizService,
            ILogger<LessonService> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _quizService = quizService;
            _logger = logger;
            _userManager = userManager;
        }

        private async Task<bool> IsStudent(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is not null) 
                return await _userManager.IsInRoleAsync(user, Roles.Student);
            return false;
        }

        private async Task<LessonScore?> GetLessonScoreAsync(Lesson lesson, string userName)
        {
            var profile = await _context.StudentProfiles
                .Include(sp => sp.SubjectSubscriptions)
                .ThenInclude(ss => ss.LessonScores)
                .ThenInclude(ls => ls.QuizScoresList)
                .FirstAsync(sp => sp.Name == userName);
            var lessonScore = profile.SubjectSubscriptions
                .Where(ss => ss.SubjectId == lesson.SubjectId)
                .SelectMany(ss => ss.LessonScores)
                .FirstOrDefault(lessonScore => lessonScore.LessonId == lesson.Id);

            if (lessonScore is not null || !await IsStudent(userName))
                return lessonScore;

            // if student and lesson score is null
            lessonScore = new LessonScore
            {
                LessonId = lesson.Id,
                IsPassed = false,
                QuizScoresList = new List<QuizScore>(),
                SubjectSubscriptionId = profile.SubjectSubscriptions.First(ss => ss.SubjectId == lesson.SubjectId).Id
            };

            _context.Entry(lessonScore).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return lessonScore;
        }

        private async Task<(Lesson?, LessonScore?)> GetLessonAndScoreAsync(int lessonId, string userName){
            var lesson = await _context.Lessons.FindAsync(lessonId);
            if (lesson is null) return (null, null);
            var lessonScore = await GetLessonScoreAsync(lesson, userName);
            return (lesson, lessonScore);
        }

        private async Task<(Lesson, LessonScore)?> GetLessonAndScoreAsync(int subjectId, int order, string userName){
            if (await _context.Lessons
                    .Include(l => l.Tests)
                    .ThenInclude(t => t.Questions)
                    .FirstOrDefaultAsync(l => l.SubjectId == subjectId && l.Order == order) is not { } lesson ||
                await GetLessonScoreAsync(lesson, userName) is not { } lessonScore)
            {
                return null;
            }

            return (lesson, lessonScore);
        }

        /// <summary>
        /// Retrieves deep lesson by assigning content and quiz array
        /// </summary>
        /// <param name="lessonId">Lesson Id to retrieve</param>
        /// <param name="userName">User name for retrieve personal information about lesson</param>
        /// <returns>Deep lesson view model with Content and Quizzes initialized. Used for LessonView component.</returns>
        public async Task<LessonViewModel?> GetLessonAsync(int lessonId, string userName)
        {
            if (await GetLessonAndScoreAsync(lessonId, userName) is not { Item1: { } } lessonInfo)
                return null;
            return MergeInLessonViewModel(lessonInfo.Item1, lessonInfo.Item2);
        }

        public async Task<LessonViewModel?> GetLessonByOrderAsync(int subjectId, int order, string userName){
            if (await GetLessonAndScoreAsync(subjectId, order, userName) is not { } lessonInfo)
                return null;
            return MergeInLessonViewModel(lessonInfo.Item1, lessonInfo.Item2);
        }
        /// <summary>
        /// Retrieves shallow lesson information just to display it in list of lessons.
        /// </summary>
        /// <param name="lessonId">Lesson Id to retrieve</param>
        /// <param name="userName">User name for retrieve personal information about lesson</param>
        /// <returns>Shallow lesson view model without Content property (string.Empty by default) and Quizzes initialized 
        /// as empty collection. Used for displaying in list of lessons </returns>
        public async Task<LessonViewModel?> GetShallowLessonAsync(int lessonId, string userName){
            if (await GetLessonAndScoreAsync(lessonId, userName) is not { Item1: { } } lessonInfo)
                return null;
            return MergeInShallowLessonViewModel(lessonInfo.Item1, lessonInfo.Item2);
        }

        private static LessonViewModel MergeInShallowLessonViewModel(Lesson lesson, LessonScore? lessonScore)
            => new(){
                LessonId = lesson.Id,
                LessonScoreId = lessonScore?.Id,
                Name = lesson.Name,
                Description = lesson.Description,
                Order = lesson.Order,
                Condition = lesson.Condition,
                SubjectId = lesson.SubjectId,
                SubscriptionId = lessonScore?.SubjectSubscriptionId,
                IsPassed = lessonScore?.IsPassed,
                Quizzes = new()
            };

        private LessonViewModel MergeInLessonViewModel(Lesson lesson, LessonScore? lessonScore)
            => new()
            {
                LessonId = lesson.Id,
                LessonScoreId = lessonScore?.Id,
                Name = lesson.Name,
                Description = lesson.Description,
                Order = lesson.Order,
                Content = lesson.Content,
                Condition = lesson.Condition,
                SubjectId = lesson.SubjectId,
                SubscriptionId = lessonScore?.SubjectSubscriptionId,
                IsPassed = lessonScore?.IsPassed,
                Quizzes = lesson.Tests.Where(quiz => quiz.Questions.Count >= quiz.Count)
                    .Select(q => _quizService.GetShallowQuizInfoAsync(q.Id, lessonScore?.Id))
                    .Select(t => t.Result)
                    .OfType<QuizViewModel>()
                    .ToList()
            };

        public Lesson? GetLessonPerEdit(int lessonId)
        => _context.Lessons
            .Include(lesson => lesson.Tests)
            .FirstOrDefault(lesson => lesson.Id == lessonId);

        #region IS_PASSED

        private static bool CheckPassed(IEnumerable<Quiz> quizzes, LessonScore lessonScore)
        {
            bool Passed = true;
            foreach (var quiz in quizzes)
            {
                Passed &= lessonScore.QuizScoresList.Any(qs => qs.QuizId == quiz.Id && qs.Score > 0);
                if (!Passed) break;
            }

            return Passed;
        }

        private static bool IsPassed(PassCondition condition, Lesson lesson, LessonScore lessonScore)
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
