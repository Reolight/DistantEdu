﻿using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using DistantEdu.Models;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Models.StudentProfileFeature;

namespace DistantEdu.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<StudentProfile> StudentProfiles { get; set; }
        public DbSet<QuizScore> QuizScores { get; set; }
        public DbSet<LessonScore> LessonScores { get; set; }
        
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        

        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<QuizScore>()
                .HasOne(qs => qs.Quiz)
                .WithMany(q => q.QuizScores);
            builder.Entity<QuizScore>()
                .HasOne(qs => qs.LessonScore)
                .WithMany(ls => ls.QuizScoresList);
        }
    }
}