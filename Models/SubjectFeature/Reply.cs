using System.ComponentModel.DataAnnotations.Schema;
using DistantEdu.Models.StudentProfileFeature;

namespace DistantEdu.Models.SubjectFeature
{
    public class Reply
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool isCorrect { get; set; }

        [ForeignKey(nameof(Query))]
        public int QueryId {get; set;}
        public Query Query { get; set; }
        public List<Replied> Replieds { get; set; } = new List<Replied>();
    }
}