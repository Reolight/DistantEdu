using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistantEdu.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LessonController : ControllerBase
    {
        
    }
}
