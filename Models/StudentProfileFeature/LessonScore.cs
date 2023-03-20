using DistantEdu.Models.SubjectFeature;

namespace DistantEdu.Models.StudentProfileFeature
{
    public class LessonScore
    {
        public int Id { get; set; }
        public Lesson PassedLesson { get; set; }
        public List<QuizScores> QuizScoresList { get; set; }
        public bool IsPassed { get; set; }
    }
}