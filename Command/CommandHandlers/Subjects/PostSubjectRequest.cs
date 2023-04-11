using DistantEdu.ViewModels;
using MediatR;

namespace DistantEdu.Command.CommandHandlers.Subjects
{
    public class PostSubjectRequest : IRequest<Unit>
    {
        public SubjectViewModel SubjectVm { get; set; } = null!;
        public string AuthorName { get; set; } = string.Empty;
    }
}
