using System;
using System.Security.Claims;
using DistantEdu.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;

namespace DistantEdu.Controllers
{
    // this template is to be future template for all controllers to reduce
    // num of actions in each method.

    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        private ApplicationDbContext? _context;
        protected ApplicationDbContext Context =>
            _context ??= HttpContext.RequestServices.GetService<ApplicationDbContext>()!;

        private IMediator? _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;

        internal string UserName => User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name : { }} userName?
            string.Empty : userName.Subject.Name;
    }
}
