using System.ComponentModel.DataAnnotations.Schema;
using DistantEdu.Models.SubjectFeature;

namespace DistantEdu.Models.StudentProfileFeature
{
    public class QuizScores
    {
        public int Id { get; set; }
        public Quiz RepliedQuiz { get; set; }
    }
}
