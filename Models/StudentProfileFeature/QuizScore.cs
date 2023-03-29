using System.ComponentModel.DataAnnotations.Schema;
using DistantEdu.Models.SubjectFeature;

namespace DistantEdu.Models.StudentProfileFeature
{
    public class QuizScore
    {
        public int Id { get; set; }

        // unic per user
        [ForeignKey(nameof(LessonScore))]
        public int LessonScoreId { get; set; }
        public virtual LessonScore LessonScore { get; set; }
        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }
        public double Score { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public List<QueryReplied> QueryReplieds { get; set; }
    }
}
