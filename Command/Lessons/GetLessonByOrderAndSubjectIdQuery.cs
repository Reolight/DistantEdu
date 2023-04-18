using DistantEdu.Data;
using DistantEdu.Models;
using DistantEdu.ViewModels;
using MediatR;

namespace DistantEdu.Command.Lessons
{
    public class GetLessonByOrderAndSubjectIdQuery : IRequest<LessonViewModel?>
    {
        public ApplicationUser User { get; set; } = null!;
        public int SubjectId { get; set; }
        public int Order { get; set; }
    }
}
