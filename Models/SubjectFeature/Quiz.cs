using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistantEdu.Models.SubjectFeature
{
    public class Quiz
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public QuizType QType { get; set; }
        public int Count { get; set; }


        [ForeignKey(nameof(Lesson))]
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public List<QuizScore> QuizScores { get; set; } = new List<QuizScore>();
        public List<Query> Questions { get; set; } = new List<Query>();
    }
}
