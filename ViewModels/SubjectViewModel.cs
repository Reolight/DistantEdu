using DistantEdu.Models.SubjectFeature;

namespace DistantEdu.ViewModels
{
    public class SubjectViewModel
    {
        public int Id { get; set; }
        public int? SubscriptionId { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<LessonViewModel> Lessons { get; set; } = new();

        public SubjectViewModel() { }
        public SubjectViewModel(Subject subject) {
            Id = subject.Id;
            Author = subject.Author;
            Name = subject.Name;
            Description = subject.Description;
        }
    }
}
