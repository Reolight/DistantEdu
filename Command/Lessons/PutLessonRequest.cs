using DistantEdu.Types;
using MediatR;

namespace DistantEdu.Command.Lessons
{
    public class PutLessonRequest : IRequest<bool>
    {
        public int Id { get; set; }
        public PassCondition Condition { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
