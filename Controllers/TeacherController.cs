using DistantEdu.Command.CommandHandlers.Quizzes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistantEdu.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TeacherController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> GetSolved (int quizId) {
            var query = new QuizInfoQuery { QuizId = quizId };
            var solvedQuizzes = await Mediator.Send(query);
            return Ok(solvedQuizzes);
        }
    }
}