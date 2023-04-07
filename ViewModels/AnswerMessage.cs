namespace DistantEdu.ViewModels
{
    public class AnswerMessage
    {
        public int QueryScoreId { get; set; }
        public List<int> SelectedReplies { get; set; } = new();
    }
}
