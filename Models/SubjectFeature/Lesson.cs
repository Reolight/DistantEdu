namespace DistantEdu.Models.SubjectFeature
{
    public class Lesson
    {
        public int Id { get; set; }

        // For making a sort of linked list. Not unic. Identical order nunbers give in result blocks of lessons
        public int Order { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // stringified content
        public string Content { get; set; }
        public List<Quiz> Tests { get; set; }
    }
}
