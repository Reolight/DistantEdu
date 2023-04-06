using DistantEdu.Models.SubjectFeature;
using DistantEdu.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistantEdu.Models.StudentProfileFeature
{
    public class QueryReplied
    {
        public int Id { get; set; }
        public bool IsReplied { get; set; }
        public CorrectGrades IsCorrect { get; set; }


        [ForeignKey(nameof(Query))]
        public int? QueryId { get; set; }
        public Query? Query { get; set; }

        [ForeignKey(nameof(StudentProfileFeature.QuizScore))]
        public int QuizScoreId { get; set; }
        public QuizScore? QuizScore { get; set; }
        public List<Replied> Answers { get; set; } = new List<Replied>();
    }
}
