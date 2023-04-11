using DistantEdu.Data;
using DistantEdu.ViewModels;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System.Composition.Convention;

namespace DistantEdu.Command.CommandHandlers.Subjects
{
    public class GetSubjectsHandler : Command<GetSubjectQuery, IEnumerable<SubjectViewModel>>
    {
        private readonly ApplicationDbContext _db;
        public GetSubjectsHandler(HttpContext context) : base(context) 
        {
            var db = context.RequestServices.GetService<ApplicationDbContext>();
            if (db is null)
            {
                throw new ArgumentNullException(nameof(db));
            }

            _db = db;
        }

        public override async Task<bool> CanExecute(GetSubjectQuery request)
        {
            return await Task.Factory.StartNew(() => !string.IsNullOrEmpty(request.Name));
        }
        public override async Task<IEnumerable<SubjectViewModel>> Execute(GetSubjectQuery request)
        {
            var subjects = await _db.Subjects
                .AsNoTracking()
                .Take(25)
                .ToListAsync();

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
