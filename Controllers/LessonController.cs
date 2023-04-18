using DistantEdu.Command.Lessons;
using DistantEdu.MessageObject;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistantEdu.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LessonController : BaseController
    {
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<LessonViewModel?>> GetLesson(int subjectId, int order)
        {
            if (_currentUser is not { })
                return Unauthorized();

            GetLessonByOrderAndSubjectIdQuery lessonQuery = new()
            {
                User = _currentUser,
                SubjectId = subjectId,
                Order = order
            };

            var lessonViewModel = await Mediator.Send(lessonQuery);
            return Ok(lessonViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> PostLesson(int subjectId, [FromBody] LessonMessage lessonMessage)
        {
            if (await Mediator.Send(new PostLessonRequest { SubjectId = subjectId, LessonMessage = lessonMessage}) is not { } lesson)
                return BadRequest($"{nameof(Lesson)} was not created");
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