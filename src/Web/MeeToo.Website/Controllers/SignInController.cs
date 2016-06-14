using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeeToo.Websiite.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
    }
}
