using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistantEdu.Data;
using DistantEdu.Models;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DistantEdu.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ILogger<SubjectController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public SubjectController(ILogger<SubjectController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<ActionResult<IEnumerable<SubjectViewModel>>> Get()
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject: { Name: { } } } UserClaims) return Unauthorized();
            var subjects = await _context.Subjects.Include(sub => sub.SubscribedUsers).ToListAsync();
            if (subjects is not { }) return new List<SubjectViewModel>();
            var viewModels = subjects.Select(sub => 
                new SubjectViewModel(sub, sub.SubscribedUsers.Any(user => user.Name == UserClaims.Subject.Name)));
            return Ok(viewModels);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [Route("{id}")]
        public async Task<ActionResult<SubjectViewModel>> Get(int id)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject: { Name: { } } } userClaims) return Unauthorized();
            var subject = await _context.Subjects
                .Include(s => s.Lessons)
                .Include(s => s.SubscribedUsers)
                .FirstAsync(s => s.Id == id);
            if (!subject.SubscribedUsers.Any(user => user.Name == userClaims.Subject.Name)) {
                var studProfile = await _context.StudentProfiles.FirstOrDefaultAsync(s => s.Name == userClaims.Subject.Name);
                if (studProfile is not { } profile) { return Ok(subject); }

                subject.SubscribedUsers.Add(profile);
                profile.SubscribedSubjects.Add(new Models.StudentProfileFeature.SubjectSubscription
                {
                    LessonScores = new(),
                    SubscribedSubject = subject
                });

                _context.SaveChanges();
            }

            subject.Lessons.OrderBy(lesson => lesson.Order);
            return Ok(subject);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Post([FromBody] SubjectViewModel subjectVm)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject: { Name: { } } } AuthorClaims) return NoContent();
            subjectVm.Author = AuthorClaims.Subject.Name;
            var subject = new Subject(subjectVm);
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
            return new CreatedAtActionResult("getbyid", "Subject", new {id = subject.Id}, subject);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [Route("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] SubjectViewModel subject)
        {
            var subjectOrig = await _context.Subjects.FirstOrDefaultAsync(x => x.Id == subject.Id);
            if (subjectOrig is null) return BadRequest();
            subjectOrig.Update(subject);
            _context.Subjects.Update(subjectOrig);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int id)
        {
            var sub = await _context.Subjects.FirstOrDefaultAsync(sub => sub.Id == id);
            if (sub is not { } subject) return new NotFoundResult();
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }
    }
}
