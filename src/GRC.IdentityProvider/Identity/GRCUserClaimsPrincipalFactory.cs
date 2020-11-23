using System.Security.Claims;
using System.Threading.Tasks;
using GRC.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace GRC.IdentityProvider.Areas.Identity
{
    public class GRCUserClaimsPrincipalFactory:
        UserClaimsPrincipalFactory<GRCUser, IdentityRole>
    {
        public GRCUserClaimsPrincipalFactory(UserManager<GRCUser> userManager,RoleManager<IdentityRole> roleManager,IOptions<IdentityOptions> options)
            : base (userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(GRCUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("Organization", user.Organization));
            identity.AddClaim(new Claim("FullName", user.FullName));
            return identity;
        }
    }
}
