namespace EventManagement.UserManagement.Web.Pages
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);

        public void OnGet()
        {
            this.RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier;
        }
    }
}