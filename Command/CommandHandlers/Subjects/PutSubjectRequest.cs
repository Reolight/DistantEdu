using DistantEdu.ViewModels;
using MediatR;

namespace DistantEdu.Command.CommandHandlers.Subjects
{
    public class PutSubjectRequest : IRequest<Unit>
    {
        public SubjectViewModel Subject { get; set; } = null!;
    }
}
