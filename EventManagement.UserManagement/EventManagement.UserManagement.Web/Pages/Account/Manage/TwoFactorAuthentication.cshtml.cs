namespace EventManagement.UserManagement.Web.Pages.Account.Manage
{
    using System;
    using System.Threading.Tasks;

    using EventManagement.UserManagement.Shared.Models;
    

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    public class TwoFactorAuthenticationModel : PageModel
    {
        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}";

        private readonly ILogger<TwoFactorAuthenticationModel> _logger;

        private readonly SignInManager<User> _signInManager;

        private readonly UserManager<User> _userManager;

        public TwoFactorAuthenticationModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<TwoFactorAuthenticationModel> logger)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._logger = logger;
        }

        public bool HasAuthenticator { get; set; }

        [BindProperty]
        public bool Is2faEnabled { get; set; }

        public int RecoveryCodesLeft { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException(
                    $"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            this.HasAuthenticator = await this._userManager.GetAuthenticatorKeyAsync(user) != null;
            this.Is2faEnabled = await this._userManager.GetTwoFactorEnabledAsync(user);
            this.RecoveryCodesLeft = await this._userManager.CountRecoveryCodesAsync(user);

            return this.Page();
        }
    }
}