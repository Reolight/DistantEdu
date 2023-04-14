using MediatR;
using DistantEdu.ViewModels;
using Microsoft.EntityFrameworkCore;
using DistantEdu.Data;
using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.Services;
using Microsoft.AspNetCore.Identity;
using DistantEdu.Models;
using DistantEdu.Models.SubjectFeature;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DistantEdu.Command.Subjects
{
    public class GetSubjectByIdHandler : IRequestHandler<GetSubjectByIdQuery, SubjectViewModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly LessonService _lessonService;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetSubjectByIdHandler(ApplicationDbContext context, LessonService lessonService, UserManager<ApplicationUser> userManager)
        {
            (_context, _lessonService, _userManager) = (context, lessonService, userManager);
        }

        private async Task<SubjectSubscription> SubscribeIfNot(StudentProfile profile, Subject subject, int Id)
        {
            var subscription = profile.SubjectSubscriptions.Find(ss => ss.SubjectId == Id);

            if (subscription is null)
            {
                subscription = new SubjectSubscription
                {
                    LessonScores = new(),
                    SubjectId = Id
                };

                profile.SubjectSubscriptions.Add(subscription);
                subject.SubjectSubscription.Add(subscription);
                await _context.SaveChangesAsync();
            }


            return subscription;
        }

        private async Task<Subject> RetrieveSubject(int id, CancellationToken cancellationToken)
            => await _context.Subjects
                .Include(s => s.Lessons)
                .AsNoTracking()
                .FirstAsync(s => s.Id == id, cancellationToken);

        private async Task<StudentProfile> RetrieveStudentProfile(string userName, CancellationToken cancellationToken)
        {
            var studProf = await _context.StudentProfiles
                    .Include(profile => profile.SubjectSubscriptions)
                    .FirstOrDefaultAsync(sp => sp.Name == userName);
            if (studProf is not null)
                return studProf;

            studProf = new StudentProfile
            {
                Name = userName,
                SubjectSubscriptions = new()
            };

            await _context.SaveChangesAsync(cancellationToken);
            return studProf;
        }

        private async Task<bool> IsAStudent(string userName)
        {
            if (await _userManager.FindByNameAsync(userName) is not { } user)
                return false;
            var roles = await _userManager.GetRolesAsync(user);
            var isStudent = await _userManager.IsInRoleAsync(user, Roles.Student);
            return isStudent;
        }

        private List<LessonViewModel> GetLessons(Subject subject, string name)
        => subject.Lessons.Select(lesson =>
                _lessonService.GetShallowLessonAsync(lesson.Id, name).Result)
                .OfType<LessonViewModel>()
                .ToList();

        private int CalculatePassProgression(List<LessonViewModel> lessonsVm)
        {
            float passed = 0f;
            foreach (var lessonVm in lessonsVm)
            {
                if (lessonVm.IsPassed is true) passed++;
            }

            return (int)(passed / lessonsVm.Count * 100);
        }
        public async Task<SubjectViewModel> Handle(GetSubjectByIdQuery query, CancellationToken cancellationToken)
        {
            var subject = await RetrieveSubject(query.Id, cancellationToken);
            StudentProfile profile = await RetrieveStudentProfile(query.UserName, cancellationToken);
            bool isStudent = await IsAStudent(query.UserName);
            SubjectViewModel subjVm = new(subject);
            if (isStudent)
                subjVm.SubscriptionId = (await SubscribeIfNot(profile, subject, query.Id)).Id;
            subjVm.Lessons = GetLessons(subject, query.UserName);
            if (isStudent && subjVm.Lessons.Count > 0)
                subjVm.Progress = CalculatePassProgression(subjVm.Lessons);
            return subjVm;
        }
    }
}
