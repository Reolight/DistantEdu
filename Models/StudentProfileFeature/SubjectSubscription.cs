using DistantEdu.Models.SubjectFeature;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistantEdu.Models.StudentProfileFeature
{
    public class SubjectSubscription
    {
        public int Id { get; set; }


        [ForeignKey(nameof(Subject))]
        public int? SubjectId { get; set; }
        public Subject? Subject { get; set; }

        [ForeignKey(nameof(StudentProfile))]
        public int StudentProfileId { get; set; }
        public StudentProfile? SubscriberProfile { get; set; }
        public List<LessonScore> LessonScores { get; set; } = new List<LessonScore>();
    }
}
