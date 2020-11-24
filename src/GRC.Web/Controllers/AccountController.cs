using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GRC.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        public IActionResult Logout() => SignOut(new[] { "Cookies", "OpenIdConnect" });

        public IActionResult Login() => Challenge(new AuthenticationProperties { RedirectUri = "/" });

        public IActionResult AccessDenied() => View();

    }
}
