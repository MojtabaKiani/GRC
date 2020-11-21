
using System.Security.Claims;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UserManagementHelper
    {
        public static bool IsAdministrator(this ClaimsPrincipal User) => User.IsInRole("Administrator");
    }
}
