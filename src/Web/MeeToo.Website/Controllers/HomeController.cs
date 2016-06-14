using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeeToo.Websiite.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "website")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View("SignIn");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
