using DistantLearningSystemReact.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomeName.Data;
using SomeName.Models;

namespace SomeName.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubjectController
    {
        private readonly ILogger<SubjectController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public SubjectController(ILogger<SubjectController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IEnumerable<Subject>> Get()
        {
            return await _context.Subjects.ToListAsync();
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Post([FromBody] Subject subject)
        {
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
            return new CreatedAtActionResult("getbyid", "Subject", new {id = subject.Id}, subject);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [Route("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Subject subject)
        {
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int id)
        {
            var sub = await _context.Subjects.FirstOrDefaultAsync(sub => sub.Id == id);
            if (sub is not { } subject) return new NotFoundResult();
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }
    }
}
