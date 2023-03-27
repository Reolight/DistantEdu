namespace DistantEdu.ViewModels
{
    public class QuestionViewModel
    {
        public int QueryId { get; set; }
        public int QueryScoreId { get; set; }
        public string Content { get; set; }
        public bool IsReplied { get; set; }
        public bool IsCorrect { get; set; }
        public List<ReplyViewModel> Replies { get; set; }
    }
}
