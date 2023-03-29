﻿using DistantEdu.Data;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Services;
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
        public async Task<ActionResult> GetLesson(int subjectId, int lessonId)
        {

            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject: { Name: { } } } userClaims)
                return Unauthorized();
            var subject = await _context.Subjects.FindAsync(subjectId);
            if (await _context.StudentProfiles.Include(p => p.SubjectSubscriptions).FirstOrDefaultAsync(profile => profile.Name == userClaims.Subject.Name) is not { } profile)
                return BadRequest("There is no student profile");
            if (profile.SubjectSubscriptions.FirstOrDefault(sub => sub.SubjectId == subjectId) is not { }) 
                return BadRequest("You not subscribed on required subject");

            var lesson = _lessonService.GetLessonPerStudent(lessonId, userClaims.Subject.Name);
            return Ok(lesson);
        }

        [HttpPost]
        public async Task<ActionResult> PostLesson(int subjectId, [FromBody] Lesson lesson)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject: { Name: { } } } userClaims) 
                return Unauthorized();
            if (await _context.Subjects.FindAsync(subjectId) is not { } subject)
                return BadRequest("Subject with given id not found");
            subject.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(PostLesson), lesson);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateLesson(int subjectId, [FromBody] Lesson lesson)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject: { Name: { } } } userClaims)
                return Unauthorized();
            _context.Entry(lesson).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteLesson(int subjectId, int lessonId) 
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject: { Name: { } } } userClaims)
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
