using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using MeeToo.Api.Models;
using MeeToo.Api.Options;
using MeeToo.Security;
using MeeToo.Security.Identity;
using MeeToo.Services.MessageSending.Email;

namespace MeeToo.Api.Controllers
{
    [Route("api/v1.1/auth")]
    public class AuthenticationController : Controller
    {
        private readonly IOptions<WebsiteCookiesOptions> cookiesOptions;
        private readonly UserManager<MeeTooUser> userManager;
        private readonly IEmailSender emailSender;
        private readonly IEmailTemplateSource emailTemplateSource;

        private WebsiteCookiesOptions WebsiteCookiesOptions => cookiesOptions.Value;

        public AuthenticationController(IOptions<WebsiteCookiesOptions> cookiesOptions,
                                        UserManager<MeeTooUser> userManager,
                                        IEmailSender emailSender,
                                        IEmailTemplateSource emailTemplateSource)
        {
            this.cookiesOptions = cookiesOptions;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.emailTemplateSource = emailTemplateSource;
        }

        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] SignInModel signInModel)
        {
            var claims = new List<Claim> {new Claim(ClaimTypes.Email, signInModel.Email)};
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            var user = await userManager.GetUserAsync(principal);

            if (user == null || !await userManager.CheckPasswordAsync(user, signInModel.Password))
            {
                return Unauthorized();
            }

            claims.AddRange(await userManager.GetClaimsAsync(user));
            claims.AddRange((await userManager.GetRolesAsync(user)).Select(role => new Claim(ClaimTypes.Role, role)));

            var identity = new ClaimsIdentity(claims, "MeeToo.api");

            await HttpContext.Authentication.SignInAsync("api", new ClaimsPrincipal(identity));

            HttpClient httpClient = new HttpClient();

            var jsonClaims = JsonConvert.SerializeObject(claims);
            var requestContent = new StringContent(jsonClaims, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(WebsiteCookiesOptions.WebsiteCookieEndpoint, requestContent);

            var enumerable = response.Headers.GetValues("Set-Cookie");

            StringValues values;
            if (Response.Headers.TryGetValue("Set-Cookie", out values))
            {
                Response.Headers["Set-Cookie"] = new StringValues(values.Concat(enumerable).ToArray());
            }
            else
            {
                Response.Headers.Add("Set-Cookie", new StringValues(enumerable.ToArray()));
            }

            return Ok();
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]UserRegistrationModel regModel)
        {
            var claims = new List<Claim> {new Claim(ClaimTypes.Email, regModel.Email)};
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            var user = await userManager.GetUserAsync(principal);

            if (user != null)
            {
                return new StatusCodeResult((int) HttpStatusCode.Conflict);
            }

            var newUser = new MeeTooUser(regModel.Email);

            try
            {
                await userManager.CreateAsync(newUser);
                await userManager.AddPasswordAsync(newUser, regModel.Password);
                await userManager.AddToRoleAsync(newUser, Roles.Student);

                var emailConfirmToken = await userManager.GenerateEmailConfirmationTokenAsync(newUser);

                await SetEmailConfirmationCookieAsync(newUser.Id);

                var templateModel = new
                {
                    Link = $"http://google.com/{emailConfirmToken}"
                };

                await emailSender.To(newUser.Email)
                                 .AddSubject("MeeToo - Email Підтвердження")
                                 .SetTemplate(emailTemplateSource, "emailConfirmation.html", templateModel)
                                 .SendAsync();
            }
            catch (Exception)
            {
                await userManager.DeleteAsync(newUser);

                throw;
            }

            return Ok();
        }

        [HttpPost]
        [Route("verifyEmail")]
        [Authorize(ActiveAuthenticationSchemes = "emailConfirmation")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var user = await userManager.GetUserAsync(User);

            var identityResult = await userManager.ConfirmEmailAsync(user, token);

            if (identityResult.Succeeded)
            {
                return Ok();
            }

            return Unauthorized();
        }

        private async Task SetEmailConfirmationCookieAsync(string userId)
        {
            var identity = new ClaimsIdentity("emailConfirmation");

            identity.AddClaim(new Claim(ClaimTypes.Email, userId));

            await HttpContext.Authentication.SignInAsync("emailConfirmation", new ClaimsPrincipal(identity));
        }
    }
}
