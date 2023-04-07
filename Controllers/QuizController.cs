﻿using System.Net.Http.Headers;
using DistantEdu.Data;
using DistantEdu.Models;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        private readonly LessonService _lessonService;
        public QuizController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, QuizService quizService, LessonService lessonService)
        {
            _context = context;
            _userManager = userManager;
            _quizService = quizService;
            _lessonService = lessonService;
        }

        [HttpPost]
        public async Task<ActionResult> PostNewQuiz([FromBody] Quiz quiz)
        {
            List<Query> queriesWithErrors = new();
            foreach (Query query in quiz.Questions)
            {
                if (query.Count > query.Replies.Count)
                    queriesWithErrors.Add(query);
            }

            quiz.Questions = quiz.Questions.Except(queriesWithErrors).ToList();
            bool isReadyForUse = quiz.Questions.Count >= quiz.Count;
            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();
            return Ok(new {
                isReady = isReadyForUse,
                removed_questions = queriesWithErrors.Select(q => q.Content)
            });
        }

        // args = quizId, lessonScoreId?
        [HttpGet]
        public async Task<ActionResult> GetQuizInfo(string args)
        {
            int[] intArray = args.Split(':').Select(int.Parse).ToArray();
            if (intArray.Length > 1)
                return Ok(await _quizService.GetShallowQuizInfoAsync(intArray[0], intArray[1]));
            return Ok(await _context.Quizzes
                                .Include(q => q.Questions)
                                .ThenInclude(qt => qt.Replies)
                                .FirstOrDefaultAsync(q => q.Id == intArray[0])
                            );
        }

        [HttpPut]
        public async Task<ActionResult> UpdateQuiz([FromBody] Quiz quiz){
            _context.Quizzes.Update(quiz);
            await _context.SaveChangesAsync();
            return Accepted();
        }
    }
}
