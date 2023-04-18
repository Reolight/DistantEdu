using DistantEdu.Models;
using MediatR;

namespace DistantEdu.Command.Lessons
{
    public class RemoveLessonRequest : IRequest<bool>
    {
        public ApplicationUser? User { get; set; } = null!;
        public int Id { get; set; }
    }
}
