using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace MeeToo.Websiite.Controllers
{
    [Route("auth/[controller]")]
    public class CookieController : Controller
    {
        [HttpPost]
        [Route("generate")]
        public async Task<IActionResult> GenerateCookies([FromBody]Claim[] claims)
        {
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.Authentication.SignInAsync("website", new ClaimsPrincipal(identity));

            return Ok();
        }
    }
}
