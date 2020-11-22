using GRC.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GRC.IdentityProvider.Infrastructure
{
    public class GRCIdentityProviderContext : IdentityDbContext<GRCUser,IdentityRole,string>
    {
        public GRCIdentityProviderContext(DbContextOptions<GRCIdentityProviderContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
