using System.ComponentModel.DataAnnotations.Schema;
using DistantEdu.Models.SubjectFeature;

namespace DistantEdu.Models.StudentProfileFeature
{
    public class Replied
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }

        [ForeignKey(nameof(Reply))]
        public int? ReplyId { get; set; }
        public Reply? Reply { get; set; }

        [ForeignKey(nameof(QueryReplied))]
        public int QueryRepliedId { get; set; }
        public QueryReplied? QueryReplied { get; set; }
    }
}
