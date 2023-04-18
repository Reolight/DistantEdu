using DistantEdu.Command.Lessons;
using DistantEdu.Data;
using DistantEdu.MessageObject;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Services;
using DistantEdu.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace DistantEdu.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LessonController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly LessonService _lessonService;
        public LessonController(ApplicationDbContext context,
            LessonService lessonService) : base()
        {
            _context = context;
            _lessonService = lessonService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<LessonViewModel?>> GetLesson(int subjectId, int order)
        {
            // if there is no subscription for the subject and user is student, then NotFound returned;
            if (!_context.StudentProfiles
                .Include(sp => sp.SubjectSubscriptions).Any(sp => sp.Name == UserName &&
                    sp.SubjectSubscriptions.Any(ss => ss.SubjectId == subjectId))
                && await isInRoleAsync(Roles.Student))
            {
                return NotFound("Subject subscription not found");
            }

            var lessonViewModel = await _lessonService.GetLessonByOrderAsync(subjectId, order, UserName);
            if (lessonViewModel is { LessonScoreId: { } } lvm)
                await _lessonService.DecideIfLessonPassedAsync((int)lvm.LessonScoreId);
            return Ok(lessonViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> PostLesson(int subjectId, [FromBody] LessonMessage lessonMessage)
        {
            if (await _context.Subjects.FindAsync(subjectId) is not { } subject)
                return BadRequest("Subject with given id not found");
            Lesson lesson = new(lessonMessage);
            subject.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(PostLesson), lesson);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateLesson(bool? contentUpdate, [FromBody] Lesson lesson)
        {
            IRequest<bool> request = contentUpdate is true ? new PutLessonContentRequest
            {
                Content = lesson.Content,
                LessonId = lesson.Id
            } :
            new PutLessonRequest
            {
                Id = lesson.Id,
                Name = lesson.Name,
                Description = lesson.Description,
                Condition = lesson.Condition
            };

            bool isSuccessful = await Mediator.Send(request);
            if (!isSuccessful) return BadRequest("Something went wrong and lesson wasn't updated");
            return Ok("Content added");
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteLesson(int lessonId)
        {
            if (await Mediator.Send(new RemoveLessonRequest { User = _currentUser, Id = lessonId }))
                return NoContent();
            return BadRequest("Lessow was not deleted because it hadn't exist.");
        }
    }
}