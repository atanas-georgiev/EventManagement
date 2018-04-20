namespace EventManagement.Portal.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Public API for user identity.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        /// <summary>
        /// Gets user information.
        /// </summary>
        /// <returns>Current user name.</returns>
        [HttpGet]        
        public IActionResult Get()
        {
            var displayUser = this.User.Identity.Name;
            return this.Json(displayUser);
        }

        /// <summary>
        /// Gets if user is admin.
        /// </summary>
        /// <returns>Current user name.</returns>
        [HttpGet("IsAdmin")]
        public IActionResult IsAdmin()
        {
            var isAdmin = this.User.IsInRole("Admin");
            return this.Json(isAdmin);
        }
    }
}
