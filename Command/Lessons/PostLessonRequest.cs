using DistantEdu.MessageObject;
using DistantEdu.Models.SubjectFeature;
using MediatR;

namespace DistantEdu.Command.Lessons
{
    public class PostLessonRequest : IRequest<Lesson?>
    {
        public int SubjectId { get; set; }

        /// <summary>
        /// Lesson message is object containing main properties of Lesson
        /// </summary>
        public LessonMessage LessonMessage { get; set; } = null!;
    }
}
