using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Types;

namespace DistantEdu.ViewModels
{
    public class SolvedQuizViewModel{
        public int QuizId { get; set; }
        public int QuizScoreId { get; set; }
        public double Score { get; set; }
        public string SolvedBy {get;set;} = string.Empty;
        public string Name { get; set; } = string.Empty;

        // Null means that quiz is not started yet
        public DateTimeOffset? StartTime { get; set; }

        // Null means that quiz is not completed
        public DateTimeOffset? EndTime { get; set; }
        public QuizType QType { get; set; }

        // Null when user in lesson menu. Filled with QuestionViewModels upon quiz creation
        public List<QueryReplied>? Questions { get; set; }


        public SolvedQuizViewModel() { }
        // Creates quizVm from Quiz
        public SolvedQuizViewModel(in Quiz quiz) { 
            QuizId = quiz.Id;
            Name  = quiz.Name;
            QType = quiz.QType;
        }

        public void MergeWithScore(in QuizScore score){
            QuizScoreId = score.Id;
            Score = score.Score;
            StartTime = score.StartTime;
            EndTime = score.EndTime;
        }

        public void SetSolverName(in string solver){
            SolvedBy = solver;
        }
    }
}