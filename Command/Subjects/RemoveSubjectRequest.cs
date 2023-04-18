using MediatR;

namespace DistantEdu.Command.Subjects
{
    public class RemoveSubjectRequest : IRequest<bool>
    {
        public int SubjectId { get; set; }
    }
}
