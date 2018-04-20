namespace EventManagement.UserManagement.Web.Pages.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.UserManagement.Shared.Extensions;
    using EventManagement.UserManagement.Shared.Models;
    using JwtAuthenticationHelper.Abstractions;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Authentication.Cookies;

    public class LoginModel : PageModel
    {
        private readonly IJwtTokenGenerator tokenGenerator;

        private readonly ILogger<LoginModel> _logger;

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public LoginModel(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<LoginModel> logger, IJwtTokenGenerator tokenGenerator)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._logger = logger;
            this.tokenGenerator = tokenGenerator;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            this.ExternalLogins = (await this._signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;

            if (this.ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                // this._signInManager.CheckPasswordSignInAsync()

                var user = this._userManager.Users.FirstOrDefault(u => u.UserName == this.Input.Email);

                if (user != null)
                {
                    var result = await this._signInManager.CheckPasswordSignInAsync(
                        user,
                        this.Input.Password, 
                        false);

                    if (result.Succeeded)
                    {
                        this._logger.LogInformation("User logged in.");
                        var accessTokenResult = this.tokenGenerator.GenerateAccessTokenWithClaimsPrincipal(
                            this.Input.Email,
                            user.GetClaims(this._userManager));
                        await this.HttpContext.SignInAsync(accessTokenResult.ClaimsPrincipal, accessTokenResult.AuthProperties);

                        return this.RedirectToPage("/Users/Index");
                    }

                    if (result.RequiresTwoFactor)
                    {
                        return this.RedirectToPage(
                            "./LoginWith2fa",
                            new { ReturnUrl = returnUrl, RememberMe = this.Input.RememberMe });
                    }

                    if (result.IsLockedOut)
                    {
                        this._logger.LogWarning("User account locked out.");
                        return this.RedirectToPage("./Lockout");
                    }
                }

                this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return this.Page();
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
    }
}