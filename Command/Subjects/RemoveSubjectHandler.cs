using DistantEdu.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistantEdu.Command.Subjects
{
    public class RemoveSubjectHandler : IRequestHandler<RemoveSubjectRequest, bool>
    {
        private readonly ApplicationDbContext _context;

        public RemoveSubjectHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(RemoveSubjectRequest request, CancellationToken cancellationToken)
        {
            var sub = await _context.Subjects.FirstOrDefaultAsync(sub => sub.Id == request.SubjectId, cancellationToken: cancellationToken);
            if (sub is not { } subject) return false;
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
