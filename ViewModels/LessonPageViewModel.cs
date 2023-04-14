using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.Models.SubjectFeature;
using DistantEdu.Types;

namespace DistantEdu.ViewModels
{
    public class LessonViewModel
    {
        public int LessonId {get;set;}
        public int? LessonScoreId { get;set;}
        public int? SubscriptionId { get;set;}
        public int SubjectId { get;set;}
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }
        public string Content { get; set; } = string.Empty;
        public PassCondition Condition { get; set; }
        public bool? IsPassed { get; set; }

        // Here selecting only full quizes
        public List<QuizViewModel> Quizzes { get; set; } = new();
    }
}
