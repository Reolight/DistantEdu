using System.ComponentModel.DataAnnotations.Schema;
using DistantEdu.Models.SubjectFeature;

namespace DistantEdu.Models.StudentProfileFeature
{
    public class QuizScore
    {
        public int Id { get; set; }
        public double Score { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }


        // unic per user
        [ForeignKey(nameof(LessonScore))]
        public int LessonScoreId { get; set; }
        public LessonScore LessonScore { get; set; }

        [ForeignKey(nameof(Quiz))]
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public List<QueryReplied> QueryReplieds { get; set; } = new List<QueryReplied>();
    }
}
