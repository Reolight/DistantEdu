namespace DistantEdu.Models.SubjectFeature
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public int Count { get; set; }
        public List<Query> Questions { get; set; }
    }
}
