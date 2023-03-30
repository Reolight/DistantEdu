using DistantEdu.Types;

namespace DistantEdu.ViewModels
{
    public class QuestionViewModel
    {
        public int QueryId { get; set; }
        public int QueryScoreId { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsReplied { get; set; }
        public CorrectGrades IsCorrect { get; set; }
        public List<ReplyViewModel> Replies { get; set; } = new();
    }
}
