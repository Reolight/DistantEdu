using MediatR;
using DistantEdu.ViewModels;
using Microsoft.EntityFrameworkCore;
using DistantEdu.Data;
using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.Services;
using Duende.IdentityServer.EntityFramework.Entities;

namespace DistantEdu.Command.CommandHandlers.Subjects
{
    public class GetSubjectByIdHandler : IRequestHandler<GetSubjectByIdQuery, SubjectViewModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly LessonService _lessonService;
        public GetSubjectByIdHandler(ApplicationDbContext context, LessonService lessonService)
            => (_context, _lessonService) = (context, lessonService);

        public async Task<SubjectViewModel> Handle(GetSubjectByIdQuery query, CancellationToken cancellationToken)
        {
            var subject = await _context.Subjects
                .Include(s => s.Lessons)
                .AsNoTracking()
                .FirstAsync(s => s.Id == query.Id);
            StudentProfile profile = await _context.StudentProfiles
                .Include(profile => profile.SubjectSubscriptions)
                .FirstOrDefaultAsync(sp => sp.Name == query.UserName)
                ?? new StudentProfile
                {
                    Name = query.UserName,
                    SubjectSubscriptions = new()
                };

            var subscription = profile.SubjectSubscriptions.Find(ss => ss.SubjectId == query.Id);

            if (subscription is null)
            {
                subscription = new SubjectSubscription
                {
                    LessonScores = new(),
                    SubjectId = query.Id
                };

                profile.SubjectSubscriptions.Add(subscription);
                subject.SubjectSubscription.Add(subscription);
            }

            await _context.SaveChangesAsync();

            return new SubjectViewModel(subject)
            {
                SubscriptionId = subscription.Id,
                Lessons = subject.Lessons.Select(lesson =>
                    _lessonService.GetShallowLessonAsync(lesson.Id, profile.Name).Result)
                        .OfType<LessonViewModel>()
                        .ToList()
            };
        }
    }
}
