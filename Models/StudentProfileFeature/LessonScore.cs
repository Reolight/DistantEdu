using DistantEdu.Models.SubjectFeature;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistantEdu.Models.StudentProfileFeature
{
    public class LessonScore
    {
        public int Id { get; set; }

        [ForeignKey(nameof(SubjectSubscription))]
        public int SubjectSubscriptionId { get; set; }

        [ForeignKey(nameof(Lesson))]
        public int LessonId { get; set; }
        public List<QuizScore> QuizScoresList { get; set; }
        public bool IsPassed { get; set; }
    }
}