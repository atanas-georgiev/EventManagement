namespace EventManagement.UserManagement.Web.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.UserManagement.Shared.Extensions;
    using EventManagement.UserManagement.Shared.Models;
    using EventManagement.UserManagement.Web.Services;

    using JwtAuthenticationHelper.Abstractions;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    public class RegisterModel : PageModel
    {
        private readonly IEmailSender _emailSender;

        private readonly ILogger<LoginModel> _logger;

        private readonly SignInManager<User> _signInManager;

        private readonly UserManager<User> _userManager;

        private readonly IJwtTokenGenerator tokenGenerator;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<LoginModel> logger,
            IEmailSender emailSender,
            IJwtTokenGenerator tokenGenerator)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._logger = logger;
            this._emailSender = emailSender;
            this.tokenGenerator = tokenGenerator;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            if (this.ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = this.Input.Email,
                    Email = this.Input.Email,
                    FirstName = this.Input.FirstName,
                    LastName = this.Input.LastName
                };

                var result = await this._userManager.CreateAsync(user, this.Input.Password);
                if (result.Succeeded)
                {
                    this._logger.LogInformation("User created a new account with password.");

                    var code = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = this.Url.EmailConfirmationLink(user.Id, code, this.Request.Scheme);
                    await this._emailSender.SendEmailConfirmationAsync(this.Input.Email, callbackUrl);

                    var accessTokenResult = this.tokenGenerator.GenerateAccessTokenWithClaimsPrincipal(
                        this.Input.Email,
                        user.GetClaims(this._userManager));
                    await this.HttpContext.SignInAsync(accessTokenResult.ClaimsPrincipal, accessTokenResult.AuthProperties);

                    // Add Roles
                    if (this._userManager.Users.Count() == 1)
                    {
                        await this._userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await this._userManager.AddToRoleAsync(user, "User");
                    }

                    return this.RedirectToPage("/Users/Index");
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            [StringLength(100)]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            [StringLength(100)]
            public string LastName { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(
                100,
                ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
        }
    }
}