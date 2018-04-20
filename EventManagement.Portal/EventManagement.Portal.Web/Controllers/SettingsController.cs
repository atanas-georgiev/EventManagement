namespace EventManagement.Portal.Web.Controllers
{
    using System.Threading.Tasks;

    using EventManagement.Portal.Business.UserSettings;
    using EventManagement.Portal.Web.ViewModels;
    using EventManagement.UserManagement.Shared.Extensions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    [Route("api/[controller]")]
    [Authorize]
    public class SettingsController : Controller
    {
        private const string CookieName = ".EventManagement.Settings";

        private readonly ISettingsService settingsService;
        
        public SettingsController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        /// <summary>
        /// Get settings of current user.
        /// </summary>
        /// <returns>Settings object.</returns>
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var userName = this.HttpContext.User.GetUserDetails()?.UserName;
            var settings = await this.settingsService.GetSettingsAsync(userName);

            // Create model
            var model = SettingsMapping.FromUserSettings(settings);

            // Issue new cookie
            this.IssueCookie(model);
            
            return this.Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SettingsVM model)
        {
            // Validate input arguments
            if (model == null)
            {
                this.ModelState.AddModelError("model", "Missing update model in request body.");
            }

            if (!this.ModelState.IsValid)
            {
                // return this.BadRequest(this.ModelState.GetErrors());
            }

            var userName = this.HttpContext.User.GetUserDetails()?.UserName;
            await this.settingsService.UpdateSettingsAsync(userName, model.Language.Value, model.DateFormat.Value);

            // Issue new cookie
            this.IssueCookie(model);
            
            return this.Json(model);
        }

        private void IssueCookie(SettingsVM model)
        {
            this.HttpContext.Response.Cookies.Append(CookieName, JsonConvert.SerializeObject(model, Formatting.None));
        }
    }
}