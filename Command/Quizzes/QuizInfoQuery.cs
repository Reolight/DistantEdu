using DistantEdu.ViewModels;
using MediatR;

namespace DistantEdu.Command.Quizzes
{
    public class QuizInfoQuery : IRequest<List<SolvedQuizViewModel>>
    {
        public int QuizId { get; set; }
    }
}
