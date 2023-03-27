using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Types;

namespace DistantEdu.ViewModels
{
    public class QuizViewModel
    {
        public int QuizId { get; set; }
        public int QuizScoreId { get; set; }
        public int Score { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }

        // Null means that quiz is not started yet
        public DateTime? StartTime { get; set; }

        // Null means that quiz is not completed
        public DateTime? EndTime { get; set; }
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
