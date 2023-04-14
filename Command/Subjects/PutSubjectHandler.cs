using DistantEdu.Data;
using DistantEdu.Models.SubjectFeature;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistantEdu.Command.Subjects
{
    public class PutSubjectHandler : IRequestHandler<PutSubjectRequest, Unit>
    {
        private readonly ApplicationDbContext _context;
        public PutSubjectHandler(ApplicationDbContext context) => _context = context;

        public async Task<Unit> Handle(PutSubjectRequest request, CancellationToken cancellationToken)
        {
            var subjectOrig = await _context.Subjects.FirstOrDefaultAsync(x => x.Id == request.Subject.Id);
            if (subjectOrig is null) return Unit.Value;
            subjectOrig.Update(request.Subject);
            _context.Subjects.Update(subjectOrig);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
