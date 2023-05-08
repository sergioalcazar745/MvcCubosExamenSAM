using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcCubosExamenSAM.Filters;
using MvcCubosExamenSAM.Services;
using System.Security.Claims;

namespace MvcCubosExamenSAM.Controllers
{
    public class ManagedController : Controller
    {
        private ServiceUsuarios service;
        public ManagedController(ServiceUsuarios service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            string token = await this.service.LoginAsync(username, password);
            if (token != null)
            {
                HttpContext.Session.SetString("token", token);
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.Name, username));
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(5)
                });
                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();
                if (TempData["id"] != null)
                {
                    string id = TempData["id"].ToString();
                    return RedirectToAction(action, controller, new { id = id });
                }
                return RedirectToAction(action, controller);

            }
            else
            {
                ViewData["Error"] = "El correo electronico no existe.";
                return View();
            }
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("token");
            return RedirectToAction("Cubos", "Cubos");
        }
    }
}

