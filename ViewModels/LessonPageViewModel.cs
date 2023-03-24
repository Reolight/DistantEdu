using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.Models.SubjectFeature;

namespace DistantEdu.ViewModels
{
    public class LessonPageViewModel
    {
        public Lesson Lesson { get; set; }
        public LessonScore? Progress { get; set; }
        public List<Quiz> Quizes { get; set; }
        public List<QuizScore> Scores { get; set; }
    }
}
