using DistantEdu.Models.SubjectFeature;

namespace DistantEdu.Models.StudentProfileFeature
{
    public class SubjectSubscription
    {
        public int Id { get; set; }
        public Subject SubscribedSubject { get; set; }        
        public List<LessonScore> LessonScores { get; set; }
    }
}
