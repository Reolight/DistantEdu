using DistantEdu.Models.SubjectFeature;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DistantEdu.Command.Lessons
{
    public class PutLessonContentRequest : IRequest<bool>
    {
        public int LessonId { get; set; }
        public string Content { get; set; }  
    }
}
