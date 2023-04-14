using DistantEdu.Data;
using DistantEdu.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistantEdu.Command.Subjects
{
    public class GetSubjectsHandler : IRequestHandler<GetSubjectQuery, IEnumerable<SubjectViewModel>>
    {
        private readonly ApplicationDbContext _db;
        public GetSubjectsHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<SubjectViewModel>> Handle(GetSubjectQuery request, CancellationToken cancellationToken)
        {
            var subjects = await _db.Subjects
                .AsNoTracking()
                .Take(25)
                .ToListAsync(cancellationToken);

            var subscriptions = _db.StudentProfiles
                .AsNoTracking()
                .Include(prof => prof.SubjectSubscriptions)
                .Where(prof => prof.Name == request.Name)
                .AsNoTracking()
                .SelectMany(prof => prof.SubjectSubscriptions)
                .ToList();

            // well, there is nothing special in subject subscription itself, just saving Id from it
            var viewModels = from subject in subjects
                             join subjectSubscription in subscriptions
                             on subject.Id equals subjectSubscription.SubjectId into subjectViewModels
                             from subjectVm in subjectViewModels.DefaultIfEmpty()
                             select new SubjectViewModel
                             {
                                 Id = subject.Id,
                                 SubscriptionId = subjectVm?.Id,
                                 Name = subject.Name,
                                 Author = subject.Author,
                                 Description = subject.Description
                             };

            return viewModels;
        }
    }
}
