using DistantEdu.ViewModels;

namespace DistantEdu.Command.CommandHandlers.Subjects
{
    public class GetSubjectQuery : IRequest<IEnumerable<SubjectViewModel>>
    {
        public string Name { get; set; } = string.Empty;
    }
}
