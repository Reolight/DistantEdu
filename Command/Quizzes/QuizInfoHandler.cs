using DistantEdu.Data;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistantEdu.Command.Quizzes
{
    public class QuizInfoHandler : IRequestHandler<QuizInfoQuery, List<SolvedQuizViewModel>>
    {
        private readonly ApplicationDbContext _context;

        public QuizInfoHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SolvedQuizViewModel>> Handle(QuizInfoQuery request, CancellationToken cancellationToken)
        {
            var quiz = await _context.Quizzes.FindAsync(request.QuizId, cancellationToken);
            ArgumentNullException.ThrowIfNull(quiz, nameof(quiz));
            List<SolvedQuizViewModel> solvedQuizzes = new();

            var quizScores = await _context.QuizScores
                .AsNoTracking()
                .Where(quizScore => quizScore.QuizId == request.QuizId)
                .ToListAsync(cancellationToken);

            var lessonScoreIds = quizScores
                .Select(quizScore => quizScore.LessonScoreId)
                .ToArray();

            var studentNames = _context.StudentProfiles
                .Join(_context.LessonScores, studentProfile => studentProfile.Id,
                    lessonScore => lessonScore.SubjectSubscription.StudentProfileId,
                    (studentProfile, lessonScore) => new { studentProfile, lessonScore })
                .Where(joinResult => lessonScoreIds.Contains(joinResult.lessonScore.Id))
                .Select(joinResult => new { joinResult.lessonScore.Id, joinResult.studentProfile.Name })
                .ToList();
            foreach (var quizScore in quizScores)
            {
                var solvedQuiz = new SolvedQuizViewModel(quiz);
                solvedQuiz.MergeWithScore(quizScore);
                if (studentNames.FirstOrDefault(joinedResult => joinedResult.Id == quizScore.LessonScoreId)?.Name is not { } name) continue;
                solvedQuiz.SetSolverName(name);
                solvedQuizzes.Add(solvedQuiz);
            }

            return solvedQuizzes;
        }
    }
}
