using System.Runtime.Intrinsics.X86;
using Duende.IdentityServer.EntityFramework.Options;
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
        //public DbSet<SubjectSubscription> Subscriptions { get; set; }
        public DbSet<LessonScore> LessonScores { get; set; }
        public DbSet<QuizScore> QuizScores { get; set; }
        
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
            builder.Entity<SubjectSubscription>()
                .HasOne(ss => ss.SubscriberProfile)
                .WithMany(sp => sp.SubjectSubscriptions)
                .HasForeignKey(ss => ss.StudentProfileId);
            builder.Entity<LessonScore>()
                .HasOne(ls => ls.SubjectSubscription)
                .WithMany(ss => ss.LessonScores)
                .HasForeignKey(ls => ls.SubjectSubscriptionId);
            builder.Entity<QuizScore>()
                .HasOne(qs => qs.LessonScore)
                .WithMany(ls => ls.QuizScoresList)
                .HasForeignKey(qs => qs.LessonScoreId);
            builder.Entity<QueryReplied>()
                .HasOne(qr => qr.QuizScore)
                .WithMany(qs => qs.QueryReplieds)
                .HasForeignKey(qr => qr.QuizScoreId);
            builder.Entity<Replied>()
                .HasOne(r => r.QueryReplied)
                .WithMany(qr => qr.Answers)
                .HasForeignKey(r => r.QueryRepliedId);

            builder.Entity<Lesson>()
                .HasOne(l => l.Subject)
                .WithMany(s => s.Lessons)
                .HasForeignKey(l => l.SubjectId);
            builder.Entity<Quiz>()
                .HasOne(q => q.Lesson)
                .WithMany(l => l.Tests)
                .HasForeignKey(q => q.LessonId);
            builder.Entity<Query>()
                .HasOne(q => q.Quiz)
                .WithMany(q => q.Questions)
                .HasForeignKey(q => q.QuizId);
            builder.Entity<Reply>()
                .HasOne(r => r.Query)
                .WithMany(q => q.Replies)
                .HasForeignKey(r => r.QueryId);

            builder.Entity<SubjectSubscription>()
                .HasOne(ss => ss.Subject)
                .WithMany(s => s.SubjectSubscription)
                .HasForeignKey(ss => ss.SubjectId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<LessonScore>()
                .HasOne(ls => ls.Lesson)
                .WithMany(l => l.LessonScores)
                .HasForeignKey(ls => ls.LessonId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<QuizScore>()
                .HasOne(qs => qs.Quiz)
                .WithMany(q => q.QuizScores)
                .HasForeignKey(qs => qs.QuizId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<QueryReplied>()
                .HasOne(qr => qr.Query)
                .WithMany(q => q.QueryReplieds)
                .HasForeignKey(qr => qr.QueryId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Replied>()
                .HasOne(r => r.Reply)
                .WithMany(r => r.Replieds)
                .HasForeignKey(r => r.ReplyId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}