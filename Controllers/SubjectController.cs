using DistantLearningSystemReact.Models;
using Microsoft.AspNetCore.Mvc;
using SomeName.Data;

namespace SomeName.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubjectController
    {
        private readonly ILogger<SubjectController> _logger;
        private readonly ApplicationDbContext _context;

        public SubjectController(ILogger<SubjectController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Subject> Get()
        {
            return (new List<Subject>() { new Subject { Description = "New subject like a math", Name = "Mathlike", Id = 1 } })
                .ToArray();
        }

        [HttpPost]
        public async Task Post([FromBody] Subject subject)
        {

        }
    }
}
