using DistantEdu.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistantEdu.Models.SubjectFeature
{
    public class Quiz
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Lesson))]
        public int LessonId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public QuizType QType { get; set; }
        public int Count { get; set; }
        public List<Query> Questions { get; set; }
    }
}
