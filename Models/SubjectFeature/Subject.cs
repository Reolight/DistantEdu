using DistantEdu.Models.StudentProfileFeature;
using DistantEdu.ViewModels;

namespace DistantEdu.Models.SubjectFeature
{
    public class Subject
    {
        public int Id { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Lesson> Lessons { get; set; } = new List<Lesson>();
        public List<SubjectSubscription> SubjectSubscription { get; set; } = new();

        public Subject() { }
        public Subject(SubjectViewModel subjectViewModel)
        {
            Id = subjectViewModel.Id;
            Author = subjectViewModel.Author;
            Name = subjectViewModel.Name;
            Description = subjectViewModel.Description;
        }

        public void Update(SubjectViewModel subjectViewModel)
        {
            Name = subjectViewModel.Name;
            Description = subjectViewModel.Description;
        }
    }
}
