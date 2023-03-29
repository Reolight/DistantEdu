using DistantEdu.Models.SubjectFeature;
using DistantEdu.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistantEdu.Models.StudentProfileFeature
{
    public class QueryReplied
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Query))]
        public int QueryId { get; set; }
        public bool isReplied { get; set; }
        public CorrectGrades isCorrect { get; set; }
        public List<Replied> Answers { get; set; }
    }
}
