using DistantEdu.Data;
using DistantEdu.Models;
using DistantEdu.Services;
using DistantEdu.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DistantEdu.Command.Lessons
{
    public class GetLessonByOrderAndSubjectIdHandler : IRequestHandler<GetLessonByOrderAndSubjectIdQuery, LessonViewModel?>
    {
        private readonly ApplicationDbContext _context;
        private readonly LessonService _lessonService;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetLessonByOrderAndSubjectIdHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager, LessonService lessonService)
        {
            _context = context;
            _userManager = userManager;
            _lessonService = lessonService;
        }

        public async Task<LessonViewModel?> Handle(GetLessonByOrderAndSubjectIdQuery request, CancellationToken cancellationToken)
        {
            // if there is no subscription for the subject and user is student, then NULL returned;
            if (!_context.StudentProfiles
                .Include(sp => sp.SubjectSubscriptions).Any(sp => sp.Name == request.User.UserName &&
                    sp.SubjectSubscriptions.Any(ss => ss.SubjectId == request.SubjectId))
                && await _userManager.IsInRoleAsync(request.User, Roles.Student))
            {
                return null;
            }

            var lessonViewModel = await _lessonService.GetLessonByOrderAsync(request.SubjectId, request.Order, request.User.UserName);
            if (lessonViewModel is { LessonScoreId: { } } lvm)
                await _lessonService.DecideIfLessonPassedAsync((int)lvm.LessonScoreId);
            return lessonViewModel;
        }
    }
}