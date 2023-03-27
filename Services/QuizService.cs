using DistantEdu.Data;
using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace DistantEdu.Services
{
    public class QuizService
    {
        #region DATA

        private readonly ApplicationDbContext? _context;
        private readonly ILogger<QuizService> _logger;
        
        public QuizService() { }
        public QuizService(ApplicationDbContext context, ILogger<QuizService> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        #region RETRIEVING_FROM_DB
        private async Task<Quiz?> RetrieveQuizFromDbAsync(int quizId, bool isShallow = false)
        {
            if (_context is not { } db) 
                throw new NullReferenceException($"{nameof(db)} is null");
            return isShallow? 
                await db.Quizzes.FindAsync(quizId) :
                await db.Quizzes
                    .Where(q => q.Id == quizId)
                    .Include(q => q.Questions)
                    .ThenInclude(quest => quest.Replies)
                    .FirstOrDefaultAsync();
        }
        private async Task<QuizScore?> RetrieveScoreFromDbAsync(int quizId, int lessonScoreId, bool isShallow = false)
        {
            if (_context is not { } db)
                throw new NullReferenceException($"{nameof(db)} is null");
            return isShallow ?
                await db.QuizScores.FirstOrDefaultAsync(qs => qs.LessonScoreId == lessonScoreId && qs.QuizId == quizId) : 
                await db.QuizScores
                .Where(qs => qs.LessonScoreId == lessonScoreId && qs.QuizId == quizId)
                .Include(qs => qs.queryReplieds)
                .ThenInclude(qr => qr.Answers)
                .FirstOrDefaultAsync();
        }

        #endregion

        #region LOGGER_WRAPPED_RETRIEVING
        private async Task<QuizViewModel?> GetQuizViewModelWrappedByLoggerAsync(int quizId, bool isShallow = false)
        {
            if (await RetrieveQuizFromDbAsync(quizId, isShallow) is not { } q)
            {
                _logger.Log(LogLevel.Error, $"The quiz with id {quizId} not found.");
                return null;
            }

            return new QuizViewModel(q);
        }

        private async Task<QuizScore?> GetQuizScoreWrappedByLoggerAsync(int quizId, int lessonScoreId, bool isShallow)
        {
            var quizScore = await RetrieveScoreFromDbAsync(quizId, lessonScoreId, isShallow);
            if (quizScore is null)
                _logger.Log(LogLevel.Debug, $"there is no quiz score yet");
            return quizScore;
        }

        private async Task<Quiz?> GetQuizWrappedByLoggerByIdAsync(int quizId)
        {
            if (_context is not { } db)
                throw new NullReferenceException("Database is null");
            return await db.Quizzes.FindAsync(quizId);
        }

        #endregion

        #region SCORE_ATTACHMENT
        private QuizViewModel AttachShallowQuizScoreToQuizVm(QuizViewModel quizVm, QuizScore quizScore)
        {
            quizVm.QuizScoreId = quizScore.Id;
            quizVm.StartTime = quizScore.StartTime;
            quizVm.EndTime = quizScore.EndTime;
            return quizVm;
        }

        private async Task<QuizViewModel> AttachShallowQuizScoreToQuizVmAsync(QuizViewModel quizVm, int lessonScoreId)
        {
            var quizScore = await GetQuizScoreWrappedByLoggerAsync(quizVm.QuizId, lessonScoreId, isShallow: true);
            if (quizScore is null)
                return quizVm;
            return AttachShallowQuizScoreToQuizVm(quizVm, quizScore);
        }

        // does not attach shallow properties
        private void AttachDeepQuizScoreVmAsync(QuizViewModel quizVm, QuizScore quizScore)
        {
            if (quizVm.Questions is null || quizVm.Questions.Count == 0) return;
            foreach (var questAnswered in quizScore.queryReplieds)
            {
                if (quizVm.Questions?.Find(q => q.QueryId == questAnswered.QueryId) is not { } questVm)
                    continue;
                questVm.IsCorrect = questAnswered.isCorrect;
                questVm.QueryScoreId = questAnswered.Id;
                questVm.IsReplied = questAnswered.isReplied;
                
                foreach(var replied in questAnswered.Answers)
                {
                    if (questVm.Replies.Find(r => r.RepliedId == replied.Id) is not { } replyVm)
                        continue;
                    replyVm.RepliedId = replied.Id;
                    replyVm.IsSelected = replied.IsSelected;
                }
            }
        }

        private void AttachQuizScoreToQuizVm(QuizViewModel quizVm, QuizScore quizScore)
        {
            AttachShallowQuizScoreToQuizVm(quizVm, quizScore);
            AttachDeepQuizScoreVmAsync(quizVm, quizScore);
        }

        private async Task<QuizViewModel> AttachQuizScoreToQuizVmAsync(QuizViewModel quizVm, int lessonScoreId)
        {
            var quizScore = await GetQuizScoreWrappedByLoggerAsync(quizVm.QuizId, lessonScoreId, isShallow: true);
            if (quizScore is null)
                return quizVm;

            AttachQuizScoreToQuizVm(quizVm, quizScore);
            return quizVm;
        }

        #endregion

        #region QUIZ_INFO

        /// <summary>
        /// Finds quest with specified Id in database and tries to find score for it.
        /// </summary>
        /// <returns>Shallow quiz info without questions inside + user score if found</returns>
        public async Task<QuizViewModel?> GetShallowQuizInfoAsync(int quizId, int lessonScoreId)
        {
            if (await GetQuizViewModelWrappedByLoggerAsync(quizId, isShallow: true) is not { } quiz)
                return null;
            return await AttachShallowQuizScoreToQuizVmAsync(quiz, lessonScoreId);
        }

        public async Task<QuizViewModel?> GetDeepQuizInfoAsync(int quizId, int lessonScoreId)
        {
            if (await GetQuizViewModelWrappedByLoggerAsync(quizId, isShallow: false) is not { } quiz)
                return null;
            return await AttachQuizScoreToQuizVmAsync(quiz, lessonScoreId);
        }

        #endregion

        #region NEW_QUIZ

        public async Task<QuizViewModel?> StartNewQuizAsync(int quizId, int lessonScoreId)
        {
            if (await GetStartedNewQuizAsync(quizId) is not { } quiz)
                return null;

            var quizScore = await SaveInDbAsync(quiz, lessonScoreId);
            AttachQuizScoreToQuizVm(quiz, quizScore);
            return quiz;
        }

        public async Task<QuizViewModel?> GetStartedNewQuizAsync(int quizId)
        {
            if (await RetrieveQuizFromDbAsync(quizId) is not { } q)
            {
                _logger.Log(LogLevel.Error, $"The quiz with id {quizId} not found.");
                return null;
            }

            var buildedQuestions = BuildQuestions(q.Questions, q.Count);

            // Finish building, starting quest and return it back
            return new QuizViewModel(q)
            {
                StartTime = DateTime.Now,
                Questions = buildedQuestions.ToList()
            };
        }        

        public IEnumerable<QuestionViewModel> BuildQuestions(List<Query> queries, int queryCount)
        {
            var randomlySelectedQueries = SelectRandomElements(queries, queryCount);
            return from query in randomlySelectedQueries
                   select new QuestionViewModel
                   {
                       QueryId = query.Id,
                       Content = query.Content,
                       IsReplied = false,
                       IsCorrect = false,
                       Replies = (from reply in SelectRandomElements(query.Replies, query.Count)
                                  select new ReplyViewModel
                                  {
                                      ReplyId = reply.Id,
                                      Content = reply.Content,
                                      IsSelected = false
                                  }).ToList()
                   };

        }

        private IEnumerable<T> SelectRandomElements<T>(List<T> elements, int countToSelect)
        {
            int[] selected = new int[countToSelect];
            Random random = new();
            for (int i = 0; i < countToSelect;)
            {
                int r = random.Next(elements.Count);
                if (selected.Contains(r))
                    continue;
                selected[i++] = r;
                yield return elements[r];
            }
        }

        #endregion

        #region SCORE_CREATION
        private async Task<QuizScore> SaveInDbAsync(QuizViewModel quizVm, int lessonScoreId)
        {
            if (_context is not { } db)
                throw new NullReferenceException("database is null");
            var quizScore = BuildQuizScoreTemplate(quizVm, lessonScoreId);
            await db.QuizScores.AddAsync(quizScore);
            await db.SaveChangesAsync();
            return quizScore;
        }

        private QuizScore BuildQuizScoreTemplate(QuizViewModel quizVm, int lessonScoreId)
        {
            return new()
            {
                LessonScoreId = lessonScoreId,
                QuizId = quizVm.QuizId,
                Score = 0,

                // nullable check just to remove warning. Must be initialized before coming there
                StartTime = quizVm.StartTime ?? DateTime.Now,
                queryReplieds = quizVm.Questions is not null ? BuildRepliedQueriesTemplate(quizVm.Questions).ToList() : new()
            };
        }

        private IEnumerable<QueryReplied> BuildRepliedQueriesTemplate(List<QuestionViewModel> questionViewModels)
        {
            foreach (QuestionViewModel questionVm in questionViewModels)
            {
                yield return new QueryReplied { 
                    isCorrect = false, 
                    isReplied = false,
                    QueryId = questionVm.QueryId,
                    Answers = BuildAnswersTemplate(questionVm.Replies).ToList()
                };
            };
        }

        private IEnumerable<Replied> BuildAnswersTemplate(List<ReplyViewModel> replies)
        {
            foreach (ReplyViewModel replyVm in replies)
            {
                yield return new Replied
                {
                    IsSelected = false,
                    ReplyId = replyVm.ReplyId
                };
            }
        }

        #endregion

        #region GET_UNFINISHED

        private async Task<List<Quiz>> GetQuizzesAsync(IEnumerable<int> quizzesId)
        {
            List<Quiz> quizzes = new();
            foreach (int id in quizzesId)
            {
                if (await GetQuizWrappedByLoggerByIdAsync(id) is not { } quiz)
                    continue;
                quizzes.Add(quiz);
            }

            return quizzes;
        }

        private IEnumerable<QuizViewModel> JoinOnQuiz(IEnumerable<QuizScore> quizScores, IEnumerable<Quiz> quizzes)
        {
            return from quizScore in quizScores
                   join quiz in quizzes on quizScore.QuizId equals quiz.Id
                   select new QuizViewModel
                   {
                       QuizId = quiz.Id,
                       QuizScoreId = quizScore.Id,
                       Name = quiz.Name,
                       Description = quiz.Description,
                       Count = quiz.Count,
                       Duration = quiz.Duration,
                       QType = quiz.QType,
                       Score = quizScore.Score,
                       EndTime = quizScore.EndTime,
                       StartTime = quizScore.StartTime,
                       Questions = (from queryScore in quizScore.queryReplieds
                                    join query in quiz.Questions on queryScore.QueryId equals query.Id
                                    select new QuestionViewModel
                                    {
                                        QueryId = query.Id,
                                        QueryScoreId = queryScore.Id,
                                        Content = query.Content,
                                        IsCorrect = queryScore.isCorrect,
                                        IsReplied = queryScore.isReplied,
                                        Replies = (from replied in queryScore.Answers
                                                   join reply in query.Replies on replied.ReplyId equals reply.Id
                                                   select new ReplyViewModel
                                                   {
                                                       ReplyId = reply.Id,
                                                       RepliedId = replied.Id,
                                                       Content = reply.Content,
                                                       IsSelected = replied.IsSelected,
                                                   }).ToList()
                                    }).ToList()
                   };
        }

        public async Task<List<QuizViewModel>> GetUnfinishedShallowQuizedById(List<QuizScore> quizScoresList)
        {
            var quizzesList = await GetQuizzesAsync(from quizScore in quizScoresList select quizScore.QuizId);
            return JoinOnQuiz(quizScoresList, quizzesList).ToList();
        }

        #endregion
    }
}