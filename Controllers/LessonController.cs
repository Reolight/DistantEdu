using DistantEdu.Data;
using DistantEdu.MessageObject;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Services;
using DistantEdu.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DistantEdu.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly LessonService _lessonService;
        public LessonController(ApplicationDbContext context, LessonService lessonService)
        {
            _context = context;
            _lessonService = lessonService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<LessonViewModel?>> GetLesson(int subjectId, int order)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } userClaims)
                return Unauthorized();
            if (!_context.StudentProfiles
                .Include(sp => sp.SubjectSubscriptions).Any(sp => sp.Name == userClaims.Subject.Name &&
                    sp.SubjectSubscriptions.Any(ss => ss.SubjectId == subjectId)))
            {
                return NotFound("Subject subscription not found");
            }

            var lessonViewModel = await _lessonService.GetLessonByOrderAsync(subjectId, order, userClaims.Subject.Name);
            return Ok(lessonViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> PostLesson(int subjectId, [FromBody] LessonMessage lessonMessage)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } userClaims)
                return Unauthorized();
            if (await _context.Subjects.FindAsync(subjectId) is not { } subject)
                return BadRequest("Subject with given id not found");
            Lesson lesson = new(lessonMessage);
            subject.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(PostLesson), lesson);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateLesson([FromBody] Lesson lesson)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } userClaims)
                return Unauthorized();
            if (await _context.Lessons.FindAsync(lesson.Id) is not { } dbLesson)
                return NotFound($"Lesson with {lesson.Id} not found");
            dbLesson.Condition = lesson.Condition;
            dbLesson.Name = lesson.Name;
            dbLesson.Description = lesson.Description;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateContent(int lessonId, [FromBody] string content){
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } userClaims)
                return Unauthorized();
            if (await _context.Lessons.FindAsync(lessonId) is not { } lesson)
                return NotFound($"Lesson with {lessonId} not found");
            lesson.Content = content;
            await _context.SaveChangesAsync();
            return Ok("Content added");
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteLesson(int subjectId, int lessonId) 
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } userClaims)
                return Unauthorized();
            if (await _context.Subjects.FindAsync(subjectId) is not { } subject)
                return BadRequest("Subject with given id not found");
            if (subject.Lessons.Remove(subject.Lessons.First(l => l.Id == lessonId)))
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }

            return BadRequest("Lessow was not deleted because it hadn't exist.");
        }
    }
}