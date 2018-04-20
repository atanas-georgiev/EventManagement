namespace EventManagement.UserManagement.Web.Pages.Account.Manage
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.UserManagement.Shared.Models;
    

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    public class GenerateRecoveryCodesModel : PageModel
    {
        private readonly ILogger<GenerateRecoveryCodesModel> _logger;

        private readonly UserManager<User> _userManager;

        public GenerateRecoveryCodesModel(
            UserManager<User> userManager,
            ILogger<GenerateRecoveryCodesModel> logger)
        {
            this._userManager = userManager;
            this._logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException(
                    $"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException(
                    $"Cannot generate recovery codes for user with ID '{user.Id}' because they do not have 2FA enabled.");
            }

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

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException(
                    $"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
            }

            var recoveryCodes = await this._userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            this.TempData["RecoveryCodes"] = recoveryCodes.ToArray();

            this._logger.LogInformation("User with ID '{UserId}' has generated new 2FA recovery codes.", user.Id);

            return this.RedirectToPage("./ShowRecoveryCodes");
        }
    }
}