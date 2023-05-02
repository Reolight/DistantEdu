using DistantEdu.Command.Subjects;
using DistantEdu.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DistantEdu.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SubjectController : BaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<ActionResult> Get()
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } UserClaims) 
                return Unauthorized();
            GetSubjectQuery query = new() { Name = UserClaims.Subject.Name };
            IEnumerable<SubjectViewModel> viewModels = await Mediator.Send(query);
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

            var subjVms = await Mediator.Send(new GetSubjectByIdQuery() { Id = id, UserName = userClaims.Subject.Name });
            return Ok(subjVms);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Post([FromBody] SubjectViewModel subjectVm)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } AuthorClaims) return NoContent();
            await Mediator.Send(new PostSubjectRequest { AuthorName = AuthorClaims.Subject.Name, SubjectVm = subjectVm });
            return new CreatedAtActionResult("Post", "Subject", subjectVm.Name, subjectVm);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [Route("{id}")]
        public async Task<ActionResult> Put([FromBody] SubjectViewModel subject)
        {
            await Mediator.Send(new PutSubjectRequest { Subject = subject });
            return new NoContentResult();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int id)
        {
            if (await Mediator.Send(new RemoveSubjectRequest { SubjectId = id }))
                return NoContent();
            return NotFound();
        }
    }
}
