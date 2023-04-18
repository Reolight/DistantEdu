using DistantEdu.Data;
using DistantEdu.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DistantEdu.Command.Lessons
{
    public class RemoveLessonHandler : IRequestHandler<RemoveLessonRequest, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public RemoveLessonHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<bool> Handle(RemoveLessonRequest request, CancellationToken cancellationToken)
        {
            if (request.User is not null && await _userManager.IsInRoleAsync(request.User, Roles.Student))
                return false;
            var removingLesson = await _context.Lessons.FindAsync(new object?[] { request.Id }, cancellationToken);
            if (removingLesson is null) 
                return false;
            _context.Lessons.Remove(removingLesson);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
