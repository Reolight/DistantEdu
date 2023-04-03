using DistantEdu.Types;

namespace DistantEdu.MessageObject
{
    public class LessonMessage
    {
        public int? Id { get; set; }
        public int Order { get; set; }
        public PassCondition Condition { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Content { get; set; }
        public int SubjectId { get; set; }
    }
}