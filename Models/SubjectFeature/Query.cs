namespace DistantEdu.Models.SubjectFeature
{
    public class Query
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public List<Reply> Replies { get; set; }
    }
}
