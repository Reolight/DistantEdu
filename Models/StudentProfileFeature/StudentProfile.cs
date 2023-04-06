namespace DistantEdu.Models.StudentProfileFeature
{
    public class StudentProfile
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<SubjectSubscription> SubjectSubscriptions { get; set; } = new List<SubjectSubscription>();

        public StudentProfile() { }
        public StudentProfile(string name)
        {
            Name = name;
        }
    }
}
