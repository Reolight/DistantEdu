using DistantEdu.ViewModels;
using MediatR;

namespace DistantEdu.Command.Subjects
{
    public class GetSubjectQuery : IRequest<IEnumerable<SubjectViewModel>>
    {
        public string Name { get; set; } = string.Empty;
    }
}
