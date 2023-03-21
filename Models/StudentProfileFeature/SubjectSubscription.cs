﻿using DistantEdu.Models.SubjectFeature;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistantEdu.Models.StudentProfileFeature
{
    public class SubjectSubscription
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Subject))]
        public int SubjectId { get; set; }
        public List<LessonScore> LessonScores { get; set; } = new();
    }
}
