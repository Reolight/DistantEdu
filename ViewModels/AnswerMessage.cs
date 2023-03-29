namespace DistantEdu.ViewModels
{
    public class AnswerMessage
    {
        public int QueryScoreId { get; set; }
        public List<int> SelectedRepliesId { get; set; } = new();
    }
}
