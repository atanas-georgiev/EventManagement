namespace EventManagement.UserManagement.Web.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using EventManagement.UserManagement.Shared.Models;
    
    using EventManagement.UserManagement.Web.Services;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class ForgotPasswordModel : PageModel
    {
        private readonly IEmailSender _emailSender;

        private readonly UserManager<User> _userManager;

        public ForgotPasswordModel(UserManager<User> userManager, IEmailSender emailSender)
        {
            this._userManager = userManager;
            this._emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (this.ModelState.IsValid)
            {
                var user = await this._userManager.FindByEmailAsync(this.Input.Email);
                if (user == null || !(await this._userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return this.RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await this._userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = this.Url.ResetPasswordCallbackLink(user.Id, code, this.Request.Scheme);
                await this._emailSender.SendResetPasswordAsync(this.Input.Email, callbackUrl);
                return this.RedirectToPage("./ForgotPasswordConfirmation");
            }

            return this.Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
    }
}