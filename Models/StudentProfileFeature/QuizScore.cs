using System.ComponentModel.DataAnnotations.Schema;
using DistantEdu.Models.SubjectFeature;

namespace DistantEdu.Models.StudentProfileFeature
{
    public class QuizScore
    {
        public int Id { get; set; }

        [ForeignKey(nameof(StudentProfile))]
        public int StudentProfileId { get; set; }

        [ForeignKey(nameof(LessonScore))]
        public int LessonScoreId { get; set; }
        public int QuizId { get; set; }
        public int Score { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
