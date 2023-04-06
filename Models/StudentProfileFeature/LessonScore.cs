﻿using DistantEdu.Models.SubjectFeature;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistantEdu.Models.StudentProfileFeature
{
    public class LessonScore
    {
        public int Id { get; set; }
        public bool IsPassed { get; set; }


        [ForeignKey(nameof(SubjectSubscription))]
        public int SubjectSubscriptionId { get; set; }
        public SubjectSubscription? SubjectSubscription { get; set; }

        [ForeignKey(nameof(Lesson))]
        public int? LessonId { get; set; }
        public Lesson? Lesson { get; set; }
        public List<QuizScore> QuizScoresList { get; set; } = new List<QuizScore>();
    }
}