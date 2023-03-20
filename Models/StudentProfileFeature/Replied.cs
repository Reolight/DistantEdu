using System.ComponentModel.DataAnnotations.Schema;
using DistantEdu.Models.SubjectFeature;

namespace DistantEdu.Models.StudentProfileFeature
{
    public class Replied
    {
        public int Id { get; set; }
        public Reply ReplyInst { get; set; }
        public bool IsSelected { get; set; }
    }
}
