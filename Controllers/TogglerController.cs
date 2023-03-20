using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DistantEdu.Models;

namespace DistantEdu.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TogglerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public TogglerController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public class ToggleAction{
            public bool TeacherChecked { get; set; }
            public string Name { get; set; }
        }

        [Route("aezakmi")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ToggleAction action)
        {
            if (User.Identity is not { Name: { } } CurrentUser) return BadRequest();
            if (action.Name.ToLower() != CurrentUser.Name.ToLower()) return ValidationProblem();
            if (await _userManager.FindByNameAsync(CurrentUser.Name) is not { } user) return NotFound();
            if (await _userManager.IsInRoleAsync(user, action.TeacherChecked? Roles.Teacher : Roles.Student))
                return NoContent();

            await _userManager.RemoveFromRoleAsync(user, action.TeacherChecked ? Roles.Student : Roles.Teacher);
            await _userManager.AddToRoleAsync(user, action.TeacherChecked ? Roles.Teacher : Roles.Student);
            await _userManager.UpdateAsync(user);
            return Ok();
        }
    }
}
