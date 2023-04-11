using MediatR;
using DistantEdu.ViewModels;

namespace DistantEdu.Command.CommandHandlers.Subjects
{
    public class GetSubjectByIdQuery : IRequest<SubjectViewModel>
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
