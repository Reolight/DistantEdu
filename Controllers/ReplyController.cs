using DistantEdu.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistantEdu.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize]
    public class ReplyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReplyController(ApplicationDbContext context)
        {
            _context = context;
        }


    }
}
