using DistantEdu.Data;
using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Types;
using DistantEdu.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace DistantEdu.Services
{
    public class QuizService
    {
        #region DATA

        private readonly ApplicationDbContext? _context;
        private readonly ILogger<QuizService> _logger;
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
            return isShallow ?
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
                await db.QuizScores
                    .OrderBy(qs => qs.StartTime)
                    .LastOrDefaultAsync(qs => qs.LessonScoreId == lessonScoreId && qs.QuizId == quizId) :
                await db.QuizScores
                .Where(qs => qs.LessonScoreId == lessonScoreId && qs.QuizId == quizId)
                .Include(qs => qs.QueryReplieds)
                .ThenInclude(qr => qr.Answers)
                .OrderBy(qs => qs.StartTime)
                .LastOrDefaultAsync();
        }

        private async Task<QuizScore?> RetrieveScoreFromDbByIdAsync(int quizScoreId, bool isShallow = false)
        {
            if (_context is not { } db)
                throw new NullReferenceException($"{nameof(db)} is null");
            return isShallow ?
                await db.QuizScores.FindAsync(quizScoreId) :
                await db.QuizScores
                    .Include(qs => qs.QueryReplieds)
                    .ThenInclude(qr => qr.Answers)
                    .FirstOrDefaultAsync(qs => qs.Id == quizScoreId);
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

        private async Task<Quiz?> GetQuizWrappedByLoggerByIdAsync(int quizId, bool isShallow = false)
        {
            var quiz = await RetrieveQuizFromDbAsync(quizId, isShallow);
            if (quiz is null)
                _logger.Log(LogLevel.Error, $"there is no such quiz [{quizId}]");
            return quiz;
        }

        private async Task<QuizScore?> GetScoreWrappedByLoggerByIdAsync(int quizScoreId, bool isShallow = false)
        {
            var quizScore = await RetrieveScoreFromDbByIdAsync(quizScoreId, isShallow);
            if (quizScore is null)
                _logger.Log(LogLevel.Error, $"there is no such quiz [{quizScoreId}]");
            return quizScore;
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
            foreach (var questAnswered in quizScore.QueryReplieds)
            {
                if (quizVm.Questions?.Find(q => q.QueryId == questAnswered.QueryId) is not { } questVm)
                    continue;
                questVm.IsCorrect = questAnswered.IsCorrect;
                questVm.QueryScoreId = questAnswered.Id;
                questVm.IsReplied = questAnswered.IsReplied;

                foreach (var replied in questAnswered.Answers)
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
        public async Task<QuizViewModel?> GetShallowQuizInfoAsync(int quizId, int? lessonScoreId)
        {
            if (await GetQuizViewModelWrappedByLoggerAsync(quizId, isShallow: true) is not { } quiz)
                return null;
            return lessonScoreId is not null? 
                await AttachShallowQuizScoreToQuizVmAsync(quiz, (int)lessonScoreId)
                : quiz;
        }

        public async Task<QuizViewModel?> GetDeepQuizInfoAsync(int quizId, int lessonScoreId)
        {
            if (await GetQuizViewModelWrappedByLoggerAsync(quizId, isShallow: false) is not { } quiz)
                return null;
            return await AttachQuizScoreToQuizVmAsync(quiz, lessonScoreId);
        }

        #endregion

        #region OLD_QUIZ_(UNFINISHED)

        private bool FindIfThereOldStartedQuiz(int quizId, int lessonScoreId){
            return _context?.QuizScores.Any(qs => qs.Id == quizId && qs.LessonScoreId == lessonScoreId && qs.EndTime == null) ?? false;
        }

        private async Task<QuizViewModel?> GetUnfinishedQuizAsync(int quizId, int lessonScoreId){
            var quizScore = await GetQuizScoreWrappedByLoggerAsync(quizId, lessonScoreId, false);
            if (quizScore?.EndTime is not null)
                return null;
            var quiz = await GetQuizWrappedByLoggerByIdAsync(quizId);
            if (quizScore is not null && quiz is not null)
                return MergeInViewModelDeep(quizScore, quiz);
            return null;
        }

        #endregion

        #region NEW_QUIZ

        public async Task<QuizViewModel?> StartQuiz(int quizId, int lessonScoreId){
            //if (FindIfThereOldStartedQuiz(quizId, lessonScoreId))
            if (await GetUnfinishedQuizAsync(quizId, lessonScoreId) is not { } unfinished)
                return await StartNewQuizAsync(quizId, lessonScoreId);
            return unfinished;
        }

        private async Task<QuizViewModel?> StartNewQuizAsync(int quizId, int lessonScoreId)
        {
            if (await GetStartedNewQuizAsync(quizId, lessonScoreId) is not { } quiz)
                return null;
            var quizScore = await SaveInDbAsync(quiz, lessonScoreId);
            AttachQuizScoreToQuizVm(quiz, quizScore);
            return quiz;
        }

        private async Task<bool> DecideIfQuizCanBeStarted(Quiz quiz, int lessonScoreId)
        {
            if (_context is not { } db || await db.LessonScores.FindAsync(lessonScoreId) is not { } lessonScore)
                return false;

            return !lessonScore.QuizScoresList
                .Any(qs => qs.QuizId == quiz.Id && (quiz.QType == QuizType.Hardcore || quiz.QType == QuizType.KeyHardcore));
        }

        private async Task<QuizViewModel?> GetStartedNewQuizAsync(int quizId, int lessonScoreId)
        {
            if (await RetrieveQuizFromDbAsync(quizId) is not { } q)
            {
                _logger.Log(LogLevel.Error, $"The quiz with id {quizId} not found.");
                return null;
            }

            if (!await DecideIfQuizCanBeStarted(q, lessonScoreId))
                return null;

            var buildedQuestions = BuildQuestions(q.Questions, q.Count);

            // Finish building, starting quest and return it back
            return new QuizViewModel(q)
            {
                StartTime = DateTimeOffset.UtcNow,
                Questions = buildedQuestions.ToList()
            };
        }

        private IEnumerable<QuestionViewModel> BuildQuestions(List<Query> queries, int queryCount)
        {
            return from query in SelectRandomElements(queries, queryCount)
                   select new QuestionViewModel
                   {
                       QueryId = query.Id,
                       Content = query.Content,
                       IsReplied = false,
                       IsCorrect = Types.CorrectGrades.Unreplied,
                       Replies = (from reply in SelectRandomElements(query.Replies, query.Count)
                                  select new ReplyViewModel
                                  {
                                      ReplyId = reply.Id,
                                      Content = reply.Content ?? string.Empty,
                                      IsSelected = false
                                  }).ToList()
                   };
        }

        private static IEnumerable<T> SelectRandomElements<T>(List<T> elements, int countToSelect)
        {
            int[] selected = new int[countToSelect];
            Array.Fill(selected, -1);
            List<T> elems = new();
            Random random = new();
            for (int i = 0; i < countToSelect;)
            {
                int r = random.Next(elements.Count);
                if (selected.Contains(r))
                    continue;
                selected[i++] = r;
                elems.Add(elements[r]);
            }

            return elems;
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
                QueryReplieds = quizVm.Questions is not null ? BuildRepliedQueriesTemplate(quizVm.Questions).ToList() : new()
            };
        }

        private IEnumerable<QueryReplied> BuildRepliedQueriesTemplate(List<QuestionViewModel> questionViewModels)
        {
            foreach (QuestionViewModel questionVm in questionViewModels)
            {
                yield return new QueryReplied {
                    IsCorrect = CorrectGrades.Unreplied,
                    IsReplied = false,
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

        #region MERGING

        // Deep merging of one quizScores and one quiz.
        // ChainDeepMerging is an extension method and can be found at the end of file;
        private QuizViewModel MergeInViewModelDeep(QuizScore quizScore, Quiz quiz)
            => new QuizViewModel()
                .MergeInViewModelShallow(quizScore, quiz)
                .ChainDeepMerging(quizScore, quiz);

        // Joins deeply Quizzes with QuizScores.
        private IEnumerable<QuizViewModel> JoinOnQuizzesDeep(IEnumerable<QuizScore> quizScores, IEnumerable<Quiz> quizzes)
        {
            return from quizScore in quizScores
                   join quiz in quizzes on quizScore.QuizId equals quiz.Id
                   select MergeInViewModelDeep(quizScore, quiz);
        }

        // Joins shallow quizzes with quizScores
        private IEnumerable<QuizViewModel> JoinOnQuizShallow(IEnumerable<QuizScore> quizScores, IEnumerable<Quiz> quizzes)
        {
            return from quizScore in quizScores
                   join quiz in quizzes on quizScore.QuizId equals quiz.Id
                   select new QuizViewModel().MergeInViewModelShallow(quizScore, quiz);
        }

        #endregion

        // ??
        #region GET_UNFINISHED_QUIZZES

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

        public async Task<List<QuizViewModel>> GetUnfinishedShallowQuizedById(List<QuizScore> quizScoresList)
        {
            var quizzesList = await GetQuizzesAsync(quizScoresList.Select(qs => qs.QuizId).OfType<int>());
            return JoinOnQuizShallow(quizScoresList, quizzesList).ToList();
        }

        #endregion

        #region FINISH_QUIZ

        private double GetScore(Query query, QueryReplied queryReplied)
        {
            int matches = 0;
            
            foreach (Replied replied in queryReplied.Answers)
            {
                var reply = query.Replies.First(repl => repl.Id == replied.ReplyId);
                if (reply.IsCorrect == replied.IsSelected)
                    matches++;
            }

            return matches / query.Count;
        }
        private CorrectGrades GetCorrectGrade(bool isStrict, in double score)
            => score switch
            {
                _ when score == 0 || (isStrict && score < 1) => CorrectGrades.Wrong,
                _ when score == 1 => CorrectGrades.Correct,
                _ => CorrectGrades.PartiallyCorrect,
            };

        private void CalculateScore(QuizScore quizScore, Quiz quiz)
        {
            // a value with range of [0; quiz.Count]
            double score = default;
            bool isStrict = quiz.QType == QuizType.Hardcore || quiz.QType == QuizType.KeyHardcore;

            foreach (var questionReplied in quizScore.QueryReplieds)
            {
                if (!questionReplied.IsReplied) {
                    questionReplied.IsReplied = true;
                    questionReplied.IsCorrect = CorrectGrades.Unreplied;
                    continue;
                }

                var query = quiz.Questions.First(q => q.Id == questionReplied.QueryId);
                var answerScore = GetScore(query, questionReplied);
                score += isStrict ? Math.Floor(answerScore) : answerScore;
                questionReplied.IsCorrect = GetCorrectGrade(isStrict, in answerScore);
            }

            // percent calculation with 2 digits precision.
            quizScore.Score = Math.Round(score / quizScore.QueryReplieds.Count * 100, 2);
            quizScore.EndTime = DateTimeOffset.UtcNow;
        }

        private async Task<bool> SaveCalculatedQuizScore(QuizScore quizScore)
        {
            if (_context is not { } db)
                throw new NullReferenceException(nameof(db));

            db.QuizScores.Update(quizScore);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<QuizViewModel?> FinishQuizAsync(int quizScoreId)
        {
            if (await GetScoreWrappedByLoggerByIdAsync(quizScoreId) is not { } quizScore)
                return null;
            if (quizScore.QuizId is not { } quizId || await GetQuizWrappedByLoggerByIdAsync(quizId) is not { } quiz)
                return null;

            CalculateScore(quizScore, quiz);
            _ = await SaveCalculatedQuizScore(quizScore);
            return MergeInViewModelDeep(quizScore, quiz);
        }

        #endregion

        #region REPLY

        public async Task<bool> Reply(int quizScoreId, IEnumerable<AnswerMessage> answeres)
        {
            if (await GetScoreWrappedByLoggerByIdAsync(quizScoreId) is not { } quizScore)
                return false;

            foreach (var answer in answeres)
                Reply(answer, quizScore);

            if (_context is not null)
                await _context.SaveChangesAsync();

            return true;
        }
        private void Reply(AnswerMessage answer, QuizScore quizScore)
        {
            if (quizScore.QueryReplieds.Find(qr => qr.Id == answer.QueryScoreId) is not { } query)
                throw new NullReferenceException(nameof(query));
            query.IsReplied = true;
            foreach (var selected in answer.SelectedReplies)
                query.Answers[selected].IsSelected = true;
        }

        #endregion
    }

    #region EXTENSION
    static class QuizVmExtension
    {
        /// <summary>
        /// Extension method for performing shallow merging of Quiz and QuizScore in View Model.
        /// </summary>
        /// <param name="quizViewModel"></param>
        /// <param name="quizScore"></param>
        /// <param name="quiz"></param>
        /// <returns></returns>
        public static QuizViewModel MergeInViewModelShallow(this QuizViewModel quizViewModel, QuizScore quizScore, Quiz quiz)
        {
            quizViewModel.QuizId = quiz.Id;
            quizViewModel.QuizScoreId = quizScore.Id;
            quizViewModel.Name = quiz.Name;
            quizViewModel.Description = quiz.Description;
            quizViewModel.Count = quiz.Count;
            quizViewModel.Duration = quiz.Duration;
            quizViewModel.QType = quiz.QType;
            quizViewModel.Score = quizScore.Score;
            quizViewModel.EndTime = quizScore.EndTime;
            quizViewModel.StartTime = quizScore.StartTime;
            quizViewModel.Questions = new();
            return quizViewModel;
        }

        /// <summary>
        /// Extension method to add deep part of Quiz and QuizScore to QuizViewModel. Used for method chain internally. The deep part is Queries (Questions) and below.
        /// </summary>
        /// <param name="quizViewModel">View model on which action is performed</param>
        /// <param name="quizScore">where replied questionsare extracted from</param>
        /// <param name="quiz">where questions definitions are extracted from</param>
        /// <returns>Deep merged View Model. NB: this method does not make shallow merge!</returns>
        public static QuizViewModel ChainDeepMerging(this QuizViewModel quizViewModel, QuizScore quizScore, Quiz quiz)
        {
            quizViewModel.Questions = (from queryScore in quizScore.QueryReplieds
                                       join query in quiz.Questions on queryScore.QueryId equals query.Id
                                       select new QuestionViewModel
                                       {
                                           QueryId = query.Id,
                                           QueryScoreId = queryScore.Id,
                                           Content = query.Content,
                                           IsCorrect = queryScore.IsCorrect,
                                           IsReplied = queryScore.IsReplied,
                                           Replies = (from replied in queryScore.Answers
                                                      join reply in query.Replies on replied.ReplyId equals reply.Id
                                                      select new ReplyViewModel
                                                      {
                                                          ReplyId = reply.Id,
                                                          RepliedId = replied.Id,
                                                          Content = reply.Content ?? string.Empty,
                                                          IsSelected = replied.IsSelected,
                                                      }).ToList()
                                       }).ToList();
            return quizViewModel;
        }
    }
    #endregion
}