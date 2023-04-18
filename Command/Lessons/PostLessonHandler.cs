using DistantEdu.Data;
using DistantEdu.Models.SubjectFeature;
using MediatR;

namespace DistantEdu.Command.Lessons
{
    public class PostLessonHandler : IRequestHandler<PostLessonRequest, Lesson?>
    {
        private readonly ApplicationDbContext _context;

        public PostLessonHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Lesson?> Handle(PostLessonRequest request, CancellationToken cancellationToken)
        {
            if (await _context.Subjects.FindAsync(new object?[] { request.SubjectId }, cancellationToken: cancellationToken) is not { } subject)
                return null;
            Lesson lesson = new(request.LessonMessage);
            subject.Lessons.Add(lesson);
            await _context.SaveChangesAsync(cancellationToken);
            return lesson;
        }
    }
}
