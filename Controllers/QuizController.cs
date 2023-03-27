﻿using AutoMapper.Configuration.Annotations;
using DistantEdu.Data;
using DistantEdu.Models;
using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DistantEdu.Controllers
{
    /// <summary>
    /// Controller for managing tests
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class QuizController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly QuizService _quizService;
        public QuizController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, QuizService quizService)
        {
            _context = context;
            _userManager = userManager;
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<ActionResult> GetActiveQuiz()
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } userClaims ||
                await _context.StudentProfiles
                    .Include(st => st.UnfinishedQuizzes)
                    .FirstOrDefaultAsync(sp => sp.Name == userClaims.Subject.Name) is not { } profile)
                return Unauthorized();

            var unfinishedQuizes = _quizService.GetUnfinishedShallowQuizedById(profile.UnfinishedQuizzes);
            return unfinishedQuizes != null ? Ok(unfinishedQuizes) : Ok();
        }

        /// <summary>
        /// HttpGet for quiz by Id
        /// </summary>
        /// <param name="quizId">If of required quiz</param>
        /// <returns>Quiz in the next cases: quiz contains more or equal Quiestions specified in Count property or less if issuer is a teacher or an admin. 
        /// Returns Unauthorised if there is no Claims. Not found if there is no such quiz or in the case if student tries to get access to quiz 
        /// with lack of questions.</returns>
        [HttpGet]
        public async Task<ActionResult> GetQuiz(int quizId)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } userClaims)
                return Unauthorized();
            var user = await _userManager.FindByNameAsync(userClaims.Subject.Name);
            var test = await _context.Quizzes.FindAsync(quizId);
            
            if (test == null || user == null) 
                return NotFound();

            if (test.Questions.Count >= test.Count)
                return Ok(test);
            
            if (await _userManager.IsInRoleAsync(user, Roles.Teacher) || await _userManager.IsInRoleAsync(user, Roles.Admin))
                return Ok(test);
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> PostQuiz([FromBody] Quiz quiz)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) is not { Subject.Name: { } } userClaims)
                return Unauthorized();
            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetQuiz), quiz);
        }
    }
}
