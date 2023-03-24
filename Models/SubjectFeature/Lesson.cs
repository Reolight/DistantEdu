using DistantEdu.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistantEdu.Models.SubjectFeature
{
    public class Lesson
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Subject))]
        public int SubjectId { get; set; }
        // For making a sort of linked list. Unic. Identical order nunbers give in result blocks of lessons
        public int Order { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // stringified content
        public string Content { get; set; }
        public PassCondition Condition { get; set; }
        public List<Quiz> Tests { get; set; }
    }
}
