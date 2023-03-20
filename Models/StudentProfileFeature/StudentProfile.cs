namespace DistantEdu.Models.StudentProfileFeature
{
    public class StudentProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubjectSubscription> SubscribedSubjects { get; set; }
    }
}
