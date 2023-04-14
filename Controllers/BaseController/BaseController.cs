using DistantEdu.Data;
using DistantEdu.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DistantEdu.Controllers
{
    // this template is to be future template for all controllers to reduce
    // num of actions in each method.

    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        private ApplicationDbContext? _context;
        private UserManager<ApplicationUser> _userManager = null!;
        private IMediator? _mediator;

        protected ApplicationDbContext Context =>
            _context ??= HttpContext.RequestServices.GetService<ApplicationDbContext>()!;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;

        internal string UserName => User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name : { }} userName?
            string.Empty : userName.Subject.Name;

        protected UserManager<ApplicationUser> UserManagerInstance =>
            _userManager ??= HttpContext.RequestServices.GetService<UserManager<ApplicationUser>>()!;

        protected ApplicationUser? _currentUser;
        protected async Task<ApplicationUser?> GetAppUserAsync() => 
            _currentUser ??= await UserManagerInstance.FindByNameAsync(UserName);

        protected async Task<bool> isInRoleAsync(string role)
            => await GetAppUserAsync() is not { } user ?
                false :
                await UserManagerInstance.IsInRoleAsync(user, role);
    }
}
