using System.ComponentModel.DataAnnotations.Schema;
using DistantEdu.Models.StudentProfileFeature;

namespace DistantEdu.Models.SubjectFeature
{
    public class Query
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Count { get; set; }

        [ForeignKey(nameof(Quiz))]
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public List<Reply> Replies { get; set; } = new List<Reply>();
        public List<QueryReplied> QueryReplieds { get; set; } = new List<QueryReplied>();
    }
}
