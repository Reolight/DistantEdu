using DistantEdu.Data;
using DistantEdu.Models.SubjectFeature;
using MediatR;

namespace DistantEdu.Command.Subjects
{
    public class PostSubjectHandler : IRequestHandler<PostSubjectRequest, Unit>
    {
        private readonly ApplicationDbContext _context;
        public PostSubjectHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PostSubjectRequest request, CancellationToken cancellationToken)
        {
            request.SubjectVm.Author = request.AuthorName;
            var subject = new Subject(request.SubjectVm);
            await _context.Subjects.AddAsync(subject, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
