namespace DistantEdu.Models.StudentProfileFeature
{
    public class StudentProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubjectSubscription> SubjectSubscriptions { get; set; } = new();
        public List<QuizScore> UnfinishedQuizzes { get; set; }


        public StudentProfile() { }
        public StudentProfile(string name)
        {
            Name = name;
        }
    }
}
