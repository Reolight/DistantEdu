using DistantEdu.Data;
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
        [Route("{lessonId}")]
        public async Task<ActionResult<LessonViewModel?>> GetLesson(int lessonId)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } userClaims)
                return Unauthorized();
            if (await _context.Lessons.FindAsync(lessonId) is not { } lesson)
                return NotFound();
            if (!_context.StudentProfiles
                .Include(sp => sp.SubjectSubscriptions).Any(sp => sp.Name == userClaims.Subject.Name &&
                    sp.SubjectSubscriptions.Any(ss => ss.SubjectId == lesson.SubjectId)))
            {
                return NotFound("Subject subscription not found");
            }

            var lessonViewModel = await _lessonService.GetLessonAsync(lessonId, userClaims.Subject.Name);
            return Ok(lessonViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> PostLesson(int subjectId, [FromBody] Lesson lesson)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } userClaims)
                return Unauthorized();
            if (await _context.Subjects.FindAsync(subjectId) is not { } subject)
                return BadRequest("Subject with given id not found");
            subject.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(PostLesson), lesson);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateLesson([FromBody] Lesson lesson)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } userClaims)
                return Unauthorized();
            _context.Entry(lesson).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
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
