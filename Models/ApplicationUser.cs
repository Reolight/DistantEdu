using DistantEdu.Models;
using DistantEdu.Models.StudentProfileFeature;
using Microsoft.AspNetCore.Identity;

namespace DistantEdu.Models
{
    public class ApplicationUser : IdentityUser
    {
        public StudentProfile? Profile { get; set; }
    }
}