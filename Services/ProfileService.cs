using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using SomeName.Models;
using System.Security.Claims;

namespace SomeName.Services
{
    public class ProfileService : IProfileService
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var appUser = await _userManager.GetUserAsync(context.Subject);
            if (appUser == null) return;
            IList<string> roles = await _userManager.GetRolesAsync(appUser);
            IList<Claim> roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(JwtClaimTypes.Role, role));
            }

            roleClaims.Add(new Claim(JwtClaimTypes.Name, appUser.UserName));
            context.IssuedClaims.AddRange(roleClaims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}
