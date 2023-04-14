using DistantEdu.Data;
using MediatR;

namespace DistantEdu.Command.Lessons
{
    public class PutLessonHandler : IRequestHandler<PutLessonRequest, bool>
    {
        private readonly ApplicationDbContext _context;
        public PutLessonHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(PutLessonRequest request, CancellationToken cancellationToken)
        {
            if (await _context.Lessons.FindAsync(new object?[] { request.Id },
                cancellationToken: cancellationToken) is not { } dbLesson)
            {
                return false;
            }

            dbLesson.Condition = request.Condition;
            dbLesson.Name = request.Name;
            dbLesson.Description = request.Description;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
