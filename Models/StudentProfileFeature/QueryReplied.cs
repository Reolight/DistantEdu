using System.ComponentModel.DataAnnotations.Schema;
using DistantEdu.Models.SubjectFeature;

namespace DistantEdu.Models.StudentProfileFeature
{
    public class QueryReplied
    {
        public int Id { get; set; }
        public Query RepliedQuery { get; set; }
        public bool isCorrect { get; set; }
        public List<Replied> Answers { get; set; }
    }
}
