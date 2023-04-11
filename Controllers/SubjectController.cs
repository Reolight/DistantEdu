using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistantEdu.Data;
using DistantEdu.Models;
using DistantEdu.Services;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DistantEdu.Command.CommandHandlers.Subjects;
using MediatR;

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
        private readonly LessonService _lessonService;
        private readonly IMediator _mediator;
        public SubjectController(ILogger<SubjectController> logger,
                                ApplicationDbContext context,
                                UserManager<ApplicationUser> userManager,
                                LessonService lessonService,
                                IMediator mediator)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _lessonService = lessonService;
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<ActionResult> Get()
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } UserClaims) 
                return Unauthorized();
            GetSubjectQuery query = new GetSubjectQuery() { Name = UserClaims.Subject.Name };
            IEnumerable<SubjectViewModel> viewModels = await _mediator.Send(query);
            return Ok(viewModels ?? new List<SubjectViewModel>());
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [Route("{id}")]
        public async Task<ActionResult<SubjectViewModel>> Get(int id)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } userClaims)
                return Unauthorized();

            var subject = await _context.Subjects
                .Include(s => s.Lessons)
                .AsNoTracking()
                .FirstAsync(s => s.Id == id);

            StudentProfile profile = await _context.StudentProfiles
                .Include(profile => profile.SubjectSubscriptions)
                .FirstOrDefaultAsync(sp => sp.Name == userClaims.Subject.Name)
                ?? new StudentProfile{
                    Name = userClaims.Subject.Name,
                    SubjectSubscriptions = new()
                };

            var subscription = profile.SubjectSubscriptions.Find(ss => ss.SubjectId == id);

            if (subscription is null) {
                subscription = new SubjectSubscription{
                    LessonScores = new(),
                    SubjectId = id
                };

                profile.SubjectSubscriptions.Add(subscription);
                subject.SubjectSubscription.Add(subscription);
            }

            await _context.SaveChangesAsync();

            var subjVms = new SubjectViewModel(subject)
            {
                SubscriptionId = subscription.Id,
                Lessons = subject.Lessons.Select(lesson =>
                    _lessonService.GetShallowLessonAsync(lesson.Id, profile.Name).Result)
                        .OfType<LessonViewModel>()
                        .ToList()
            };

            return Ok(subjVms);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Post([FromBody] SubjectViewModel subjectVm)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } AuthorClaims) return NoContent();
            subjectVm.Author = AuthorClaims.Subject.Name;
            var subject = new Subject(subjectVm);
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
            return new CreatedAtActionResult("getbyid", "Subject", new {id = subject.Id}, subject);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [Route("{id}")]
        public async Task<ActionResult> Put([FromBody] SubjectViewModel subject)
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
