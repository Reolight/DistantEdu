using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Types;

namespace DistantEdu.ViewModels
{
    public class QuizViewModel
    {
        public int QuizId { get; set; }
        public int? QuizScoreId { get; set; }
        public double? Score { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Duration { get; set; }

        // Null means that quiz is not started yet
        public DateTimeOffset? StartTime { get; set; }

        // Null means that quiz is not completed
        public DateTimeOffset? EndTime { get; set; }
        public QuizType QType { get; set; }
        public int Count { get; set; }

        // Null when user in lesson menu. Filled with QuestionViewModels upon quiz creation
        public List<QuestionViewModel>? Questions { get; set; }


        public QuizViewModel() { }
        // Creates quizVm from Quiz
        public QuizViewModel(Quiz quiz) { 
            QuizId = quiz.Id;
            Name  = quiz.Name;
            Description = quiz.Description;
            Duration = quiz.Duration;
            QType = quiz.QType;
            Count = quiz.Count;
        }
    }
}
