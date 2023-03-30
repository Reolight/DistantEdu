using DistantEdu.Models.SubjectFeature;

namespace DistantEdu.ViewModels
{
    public class SubjectViewModel
    {
        public int Id { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsSubscribed { get; set; } = false;

        public SubjectViewModel() { }
        public SubjectViewModel(Subject subject, bool subscribed) {
            Id = subject.Id;
            Author = subject.Author;
            Name = subject.Name;
            Description = subject.Description;
            IsSubscribed = subscribed;
        }
    }
}
