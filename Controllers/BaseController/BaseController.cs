using System;
using System.Security.Claims;
using DistantEdu.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;

namespace DistantEdu.Controllers.BaseController
{
    // this template is to be future template for all controllers to reduce
    // num of actions in each method.

    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        private ApplicationDbContext _context;
        protected ApplicationDbContext Context =>
            _context ??= HttpContext.RequestServices.GetService<ApplicationDbContext>()!;

        internal Guid UserId => !User.Identity.IsAuthenticated
            ? Guid.Empty 
            : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    }
}
