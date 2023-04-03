using System;
using DistantEdu.Types;
using DistantEdu.Models.StudentProfileFeature;
using System.ComponentModel.DataAnnotations.Schema;
using DistantEdu.MessageObject;

namespace DistantEdu.Models.SubjectFeature
{
    public class Lesson
    {
        public int Id { get; set; }

        // For making a sort of linked list. Unic. Identical order nunbers give in result blocks of lessons
        public int Order { get; set; }
        public PassCondition Condition { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<Quiz> Tests { get; set; } = new List<Quiz>();

        [ForeignKey(nameof(Subject))]
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
        public List<LessonScore> LessonScores {get; set;} = new List<LessonScore>();

        public Lesson() { }
        public Lesson(LessonMessage lessonMessage){
            Order = lessonMessage.Order;
            Condition = lessonMessage.Condition;
            Name = lessonMessage.Name;
            Description = lessonMessage.Description;
            Content = lessonMessage.Content ?? string.Empty;
            SubjectId = lessonMessage.SubjectId;
        }
    }
}
