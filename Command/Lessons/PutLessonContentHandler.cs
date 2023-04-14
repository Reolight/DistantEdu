using DistantEdu.Data;
using DistantEdu.Models.SubjectFeature;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistantEdu.Command.Lessons
{
    public class PutLessonContentHandler : IRequestHandler<PutLessonContentRequest, bool>
    {
        private readonly ApplicationDbContext _context;

        public PutLessonContentHandler(ApplicationDbContext context)
            =>  (_context) = (context);

        public async Task<bool> Handle(PutLessonContentRequest request, CancellationToken cancellationToken)
        {
            if (await _context.Lessons.FirstOrDefaultAsync(lesson => lesson.Id == request.LessonId, cancellationToken) is not { } dbLesson)
                return false;
            dbLesson.Content = request.Content;
            _context.Entry(dbLesson).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
