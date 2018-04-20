namespace EventManagement.UserManagement.Web.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using EventManagement.UserManagement.Shared.Models;
    

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    public class EnableAuthenticatorModel : PageModel
    {
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        private readonly ILogger<EnableAuthenticatorModel> _logger;

        private readonly UrlEncoder _urlEncoder;

        private readonly UserManager<User> _userManager;

        public EnableAuthenticatorModel(
            UserManager<User> userManager,
            ILogger<EnableAuthenticatorModel> logger,
            UrlEncoder urlEncoder)
        {
            this._userManager = userManager;
            this._logger = logger;
            this._urlEncoder = urlEncoder;
        }

        public string AuthenticatorUri { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public string SharedKey { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException(
                    $"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            await this.LoadSharedKeyAndQrCodeUriAsync(user);

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException(
                    $"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadSharedKeyAndQrCodeUriAsync(user);
                return this.Page();
            }

            // Strip spaces and hypens
            var verificationCode = this.Input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await this._userManager.VerifyTwoFactorTokenAsync(
                                      user,
                                      this._userManager.Options.Tokens.AuthenticatorTokenProvider,
                                      verificationCode);

            if (!is2faTokenValid)
            {
                this.ModelState.AddModelError("Input.Code", "Verification code is invalid.");
                await this.LoadSharedKeyAndQrCodeUriAsync(user);
                return this.Page();
            }

            await this._userManager.SetTwoFactorEnabledAsync(user, true);
            this._logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", user.Id);

            var recoveryCodes = await this._userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            this.TempData["RecoveryCodes"] = recoveryCodes.ToArray();
            return this.RedirectToPage("./ShowRecoveryCodes");
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }

            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                this._urlEncoder.Encode("EventManagement.UserManagement.Web"),
                this._urlEncoder.Encode(email),
                unformattedKey);
        }

        private async Task LoadSharedKeyAndQrCodeUriAsync(User user)
        {
            // Load the authenticator key & QR code URI to display on the form
            var unformattedKey = await this._userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await this._userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await this._userManager.GetAuthenticatorKeyAsync(user);
            }

            this.SharedKey = this.FormatKey(unformattedKey);
            this.AuthenticatorUri = this.GenerateQrCodeUri(user.Email, unformattedKey);
        }

        public class InputModel
        {
            [Required]
            [StringLength(
                7,
                ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Verification Code")]
            public string Code { get; set; }
        }
    }
}