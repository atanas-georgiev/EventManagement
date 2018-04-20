namespace EventManagement.UserManagement.Web.Pages.Account.Manage
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class ShowRecoveryCodesModel : PageModel
    {
        public string[] RecoveryCodes { get; private set; }

        public IActionResult OnGet()
        {
            this.RecoveryCodes = (string[])this.TempData["RecoveryCodes"];
            if (this.RecoveryCodes == null)
            {
                return this.RedirectToPage("TwoFactorAuthentication");
            }

            return this.Page();
        }
    }
}