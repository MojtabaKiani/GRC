using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GRC.Web.Extensions
{
    public static class UserManagementHelper
    {
        public static bool IsAdministrator(this ClaimsPrincipal User) => User.IsInRole("Administrator");
    }
}
