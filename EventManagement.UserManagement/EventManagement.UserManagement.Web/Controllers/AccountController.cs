namespace EventManagement.UserManagement.Web.Controllers
{
    using System.Threading.Tasks;

    using EventManagement.UserManagement.Shared.Models;
    
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication;

    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly ILogger _logger;

        private readonly SignInManager<User> _signInManager;

        public AccountController(SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            this._signInManager = signInManager;
            this._logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            this._logger.LogInformation("User logged out.");
            return this.RedirectToPage("/Account/Login");
        }
    }
}