namespace DistantEdu.ViewModels
{
    public class ReplyViewModel
    {
        // Id of an original Reply variant 
        public int ReplyId { get; set; }

        // Id of replied instance
        public int RepliedId { get; set; }
        public string Content { get; set; }
        public bool IsSelected { get; set; }
    }
}
