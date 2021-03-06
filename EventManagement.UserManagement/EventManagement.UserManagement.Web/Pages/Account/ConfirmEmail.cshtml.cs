namespace EventManagement.UserManagement.Web.Pages.Account
{
    using System;
    using System.Threading.Tasks;

    using EventManagement.UserManagement.Shared.Models;
    
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public ConfirmEmailModel(UserManager<User> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return this.RedirectToPage("/Index");
            }

            var user = await this._userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            var result = await this._userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Error confirming email for user with ID '{userId}':");
            }

            return this.Page();
        }
    }
}