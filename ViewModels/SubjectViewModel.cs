using DistantEdu.Models.SubjectFeature;

namespace DistantEdu.ViewModels
{
    public class SubjectViewModel
    {
        public int Id { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Lesson> Lessons { get; set; }
        public bool IsSubscribed { get; set; } = false;

        public SubjectViewModel() { }
        public SubjectViewModel(Subject subject, bool subscribed) {
            Id = subject.Id;
            Author = subject.Author;
            Name = subject.Name;
            Description = subject.Description;
            Lessons = new List<Lesson>();
            IsSubscribed = subscribed;
        }
    }
}
